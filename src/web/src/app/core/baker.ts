export interface HourlyCost {
  weeksPerMonth: number;
  hoursPerMonth: number;
  labour: number;
  overhead: number;
  total: number;
}

export interface BakerRequest {
  version: number;
  monthlyIncome: number;
  hoursPerDay: number;
  daysPerWeek: number;
  monthlyFixedCosts: number;
  defaultMarkup: number;
  cardFee: number;
  tax: number;
}

export interface Baker extends BakerRequest {
  hourlyCost: HourlyCost;
}

export const NO_HOURLY_COST: HourlyCost = {
  weeksPerMonth: 0,
  hoursPerMonth: 0,
  labour: 0,
  overhead: 0,
  total: 0,
};
