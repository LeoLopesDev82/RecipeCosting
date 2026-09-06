import { Component, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { EMPTY, catchError, debounceTime, filter, from, switchMap } from 'rxjs';
import { Baker as BakerSettings, BakerRequest, NO_HOURLY_COST } from '../../core/baker';
import { BakerService } from '../../core/baker.service';
import { Decimal } from '../../core/decimal.directive';

const BLANK = {
  monthlyIncome: '',
  hoursPerDay: '',
  daysPerWeek: '',
  monthlyFixedCosts: '',
  defaultMarkup: '',
  cardFee: '',
  tax: '',
};

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

  protected readonly form = inject(FormBuilder).nonNullable.group(BLANK);

  private readonly previewed = toSignal(
    this.form.valueChanges.pipe(
      debounceTime(300),
      filter(() => this.form.valid),
      switchMap(() => from(this.baker.preview(this.toRequest())).pipe(catchError(() => EMPTY))),
    ),
    { initialValue: null },
  );

  protected readonly cost = computed(
    () => this.previewed() ?? this.stored()?.hourlyCost ?? NO_HOURLY_COST,
  );

  constructor() {
    void this.load();
  }

  protected async save(): Promise<void> {
    if (this.saving()) return;

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

  private toRequest(): BakerRequest {
    const values = this.form.getRawValue();

    return {
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
