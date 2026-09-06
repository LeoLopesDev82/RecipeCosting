export interface BakerSettings {
  monthlyIncome: number;
  hoursPerDay: number;
  daysPerWeek: number;
  monthlyFixedCosts: number;
  defaultMarkup: number;
  cardFee: number;
  tax: number;
}

export interface HourlyCost {
  hoursPerMonth: number;
  labour: number;
  overhead: number;
  total: number;
}

export const WEEKS_PER_MONTH = 4.33;

export function hourlyCostOf(settings: BakerSettings): HourlyCost {
  const hoursPerMonth = settings.hoursPerDay * settings.daysPerWeek * WEEKS_PER_MONTH;
  const labour = finite(settings.monthlyIncome / hoursPerMonth);
  const overhead = finite(settings.monthlyFixedCosts / hoursPerMonth);

  return { hoursPerMonth: finite(hoursPerMonth), labour, overhead, total: labour + overhead };
}

function finite(value: number): number {
  return Number.isFinite(value) ? value : 0;
}
