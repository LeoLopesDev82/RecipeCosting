import { Component, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, debounceTime, from, map, of, switchMap, tap } from 'rxjs';
import { Baker as BakerSettings, BakerRequest, HourlyCost, NO_HOURLY_COST } from '../../core/baker';
import { BakerService } from '../../core/baker.service';
import { Decimal } from '../../core/decimal.directive';
import { between, sumBelow } from '../../core/numeric.validators';

const INVALID = 'These numbers cannot be priced. The cost below is the last one that could.';

interface Preview {
  cost: HourlyCost | null;
  problem: string | null;
}

const NOTHING_YET: Preview = { cost: null, problem: null };

@Component({
  selector: 'app-baker',
  imports: [CurrencyPipe, DecimalPipe, ReactiveFormsModule, Decimal],
  templateUrl: './baker.html',
  styleUrl: './baker.css',
})
export class Baker {
  private readonly baker = inject(BakerService);

  protected readonly loading = signal(true);
  protected readonly saving = signal(false);
  protected readonly failure = signal<string | null>(null);
  protected readonly notice = signal<string | null>(null);

  private readonly stored = signal<BakerSettings | null>(null);
  private readonly lastGood = signal<HourlyCost | null>(null);

  protected readonly form = inject(FormBuilder).nonNullable.group(
    {
      monthlyIncome: ['', between(0.01, 1_000_000)],
      hoursPerDay: ['', between(0.5, 24)],
      daysPerWeek: ['', between(1, 7)],
      monthlyFixedCosts: ['', between(0, 1_000_000)],
      defaultMarkup: ['', between(0, 1000)],
      cardFee: ['', between(0, 100)],
      tax: ['', between(0, 100)],
    },
    { validators: sumBelow(100, 'cardFee', 'tax') },
  );

  private readonly preview = toSignal(
    this.form.valueChanges.pipe(
      debounceTime(200),
      switchMap(() => (this.form.invalid ? of(refused()) : this.ask())),
    ),
    { initialValue: NOTHING_YET },
  );

  protected readonly cost = computed(
    () =>
      this.preview().cost ?? this.lastGood() ?? this.stored()?.hourlyCost ?? NO_HOURLY_COST,
  );

  protected readonly problem = computed(() => this.preview().problem);

  constructor() {
    void this.load();
  }

  protected async save(): Promise<void> {
    if (this.form.invalid || this.saving()) return;

    this.saving.set(true);
    this.failure.set(null);
    this.notice.set(null);

    try {
      this.stored.set(await this.baker.save(this.toRequest()));

      this.notice.set('Settings saved.');
    } catch (failure) {
      this.failure.set((failure as Error).message);
    }

    this.saving.set(false);
  }

  // #region Private methods

  private ask() {
    return from(this.baker.preview(this.toRequest())).pipe(
      tap(cost => this.lastGood.set(cost)),
      map(cost => ({ cost, problem: null })),
      catchError((failure: Error) => of({ cost: null, problem: failure.message })),
    );
  }

  private toRequest(): BakerRequest {
    const values = this.form.getRawValue();

    return {
      version: this.stored()?.version ?? 0,
      monthlyIncome: Number(values.monthlyIncome),
      hoursPerDay: Number(values.hoursPerDay),
      daysPerWeek: Number(values.daysPerWeek),
      monthlyFixedCosts: Number(values.monthlyFixedCosts),
      defaultMarkup: Number(values.defaultMarkup),
      cardFee: Number(values.cardFee),
      tax: Number(values.tax),
    };
  }

  private async load(): Promise<void> {
    try {
      const settings = await this.baker.load();

      this.stored.set(settings);
      this.form.setValue({
        monthlyIncome: String(settings.monthlyIncome),
        hoursPerDay: String(settings.hoursPerDay),
        daysPerWeek: String(settings.daysPerWeek),
        monthlyFixedCosts: String(settings.monthlyFixedCosts),
        defaultMarkup: String(settings.defaultMarkup),
        cardFee: String(settings.cardFee),
        tax: String(settings.tax),
      });
    } catch (failure) {
      this.failure.set((failure as Error).message);
    }

    this.loading.set(false);
  }

  // #endregion
}

function refused(): Preview {
  return { cost: null, problem: INVALID };
}
