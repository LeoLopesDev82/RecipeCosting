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

export function costOfUse(ingredient: Ingredient, quantity: number): number {
  return (ingredient.packagePrice * quantity) / ingredient.packageSize;
}

const BASE: Record<PackageUnit, { divisor: number; label: string }> = {
  g: { divisor: 1000, label: 'kg' },
  ml: { divisor: 1000, label: 'L' },
  un: { divisor: 1, label: 'unit' },
};

export function previewUnitCost(request: IngredientRequest): UnitCost {
  const base = BASE[request.packageUnit];
  const amount = (request.packagePrice * base.divisor) / request.packageSize;

  return { amount: Number.isFinite(amount) ? amount : 0, label: base.label };
}
