export type PackageUnit = 'g' | 'ml' | 'un';

export interface UnitCost {
  amount: number;
  label: string;
}

export interface Ingredient {
  id: number;
  name: string;
  packageSize: number;
  packageUnit: PackageUnit;
  packagePrice: number;
  unitCost: UnitCost;
}

export interface IngredientRequest {
  name: string;
  packageSize: number;
  packageUnit: PackageUnit;
  packagePrice: number;
}

export const NO_UNIT_COST: UnitCost = { amount: 0, label: '' };
