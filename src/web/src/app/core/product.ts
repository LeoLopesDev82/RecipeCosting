import { BakerSettings, hourlyCostOf } from './baker';
import { Ingredient, costOfUse } from './ingredient';

export interface ProductLine {
  ingredientId: number;
  quantity: number;
}

export interface Product {
  id: number;
  name: string;
  prepMinutes: number;
  markup: number | null;
  lines: ProductLine[];
}

export interface ProductCost {
  ingredients: number;
  labour: number;
  total: number;
  markup: number;
  inherited: boolean;
  price: number;
}

export type Pantry = Map<number, Ingredient>;

export function lineCostOf(line: ProductLine, pantry: Pantry): number {
  const ingredient = pantry.get(line.ingredientId);

  return ingredient ? costOfUse(ingredient, line.quantity) : 0;
}

export function productCostOf(
  product: Product,
  settings: BakerSettings,
  pantry: Pantry,
): ProductCost {
  const ingredients = product.lines.reduce((sum, line) => sum + lineCostOf(line, pantry), 0);
  const labour = (product.prepMinutes / 60) * hourlyCostOf(settings).total;
  const total = ingredients + labour;
  const markup = product.markup ?? settings.defaultMarkup;
  const fees = (settings.cardFee + settings.tax) / 100;

  return {
    ingredients,
    labour,
    total,
    markup,
    inherited: product.markup === null,
    price: (total * (1 + markup / 100)) / (1 - fees),
  };
}
