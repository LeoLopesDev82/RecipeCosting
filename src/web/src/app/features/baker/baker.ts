import { Component, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { WEEKS_PER_MONTH, hourlyCostOf } from '../../core/baker';
import { BakerService } from '../../core/baker.service';
import { Decimal } from '../../core/decimal.directive';

const EMPTY = {
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

  protected readonly weeksPerMonth = WEEKS_PER_MONTH;
  protected readonly loading = signal(true);

  protected readonly form = inject(FormBuilder).nonNullable.group(EMPTY);

  private readonly draft = toSignal(this.form.valueChanges, { initialValue: EMPTY });

  protected readonly cost = computed(() => {
    const draft = { ...EMPTY, ...this.draft() };

    return hourlyCostOf({
      monthlyIncome: Number(draft.monthlyIncome),
      hoursPerDay: Number(draft.hoursPerDay),
      daysPerWeek: Number(draft.daysPerWeek),
      monthlyFixedCosts: Number(draft.monthlyFixedCosts),
      defaultMarkup: Number(draft.defaultMarkup),
      cardFee: Number(draft.cardFee),
      tax: Number(draft.tax),
    });
  });

  constructor() {
    void this.load();
  }

  private async load(): Promise<void> {
    const settings = await this.baker.load();

    this.form.setValue({
      monthlyIncome: String(settings.monthlyIncome),
      hoursPerDay: String(settings.hoursPerDay),
      daysPerWeek: String(settings.daysPerWeek),
      monthlyFixedCosts: String(settings.monthlyFixedCosts),
      defaultMarkup: String(settings.defaultMarkup),
      cardFee: String(settings.cardFee),
      tax: String(settings.tax),
    });

    this.loading.set(false);
  }
}
