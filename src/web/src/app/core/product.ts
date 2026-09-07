import { PackageUnit } from './ingredient';

export interface ProductLine {
  ingredientId: number;
  ingredientName: string;
  quantity: number;
  unit: PackageUnit;
  cost: number;
}

export interface ProductCost {
  ingredients: number;
  labour: number;
  total: number;
  markup: number;
  inherited: boolean;
  price: number;
}

export interface Product {
  id: number;
  version: number;
  name: string;
  prepMinutes: number;
  markup: number | null;
  lines: ProductLine[];
  cost: ProductCost;
}

export interface ProductLineRequest {
  ingredientId: number;
  quantity: number;
}

export interface ProductRequest {
  version: number;
  name: string;
  prepMinutes: number;
  markup: number | null;
  lines: ProductLineRequest[];
}

export const NO_COST: ProductCost = {
  ingredients: 0,
  labour: 0,
  total: 0,
  markup: 0,
  inherited: true,
  price: 0,
};
