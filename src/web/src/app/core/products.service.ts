import { Injectable } from '@angular/core';
import { Product } from './product';

const PRODUCTS: Product[] = [
  {
    id: 1,
    name: 'Brigadeiro',
    prepMinutes: 2,
    markup: null,
    lines: [
      { ingredientId: 1, quantity: 18 },
      { ingredientId: 17, quantity: 2 },
      { ingredientId: 6, quantity: 1 },
      { ingredientId: 28, quantity: 4 },
    ],
  },
  {
    id: 2,
    name: 'Beijinho',
    prepMinutes: 2,
    markup: null,
    lines: [
      { ingredientId: 1, quantity: 18 },
      { ingredientId: 23, quantity: 6 },
      { ingredientId: 6, quantity: 1 },
    ],
  },
  {
    id: 3,
    name: 'Chocolate truffle',
    prepMinutes: 3,
    markup: null,
    lines: [
      { ingredientId: 18, quantity: 12 },
      { ingredientId: 5, quantity: 6 },
      { ingredientId: 6, quantity: 1 },
    ],
  },
  {
    id: 4,
    name: 'Chocolate bonbon',
    prepMinutes: 4,
    markup: null,
    lines: [
      { ingredientId: 19, quantity: 15 },
      { ingredientId: 1, quantity: 8 },
      { ingredientId: 6, quantity: 1 },
    ],
  },
  {
    id: 5,
    name: 'Brownie',
    prepMinutes: 4,
    markup: null,
    lines: [
      { ingredientId: 18, quantity: 15 },
      { ingredientId: 6, quantity: 12 },
      { ingredientId: 12, quantity: 20 },
      { ingredientId: 9, quantity: 12 },
      { ingredientId: 17, quantity: 5 },
      { ingredientId: 30, quantity: 0.3 },
    ],
  },
  {
    id: 6,
    name: 'Cupcake',
    prepMinutes: 5,
    markup: null,
    lines: [
      { ingredientId: 9, quantity: 30 },
      { ingredientId: 12, quantity: 25 },
      { ingredientId: 6, quantity: 15 },
      { ingredientId: 30, quantity: 0.5 },
      { ingredientId: 3, quantity: 20 },
      { ingredientId: 31, quantity: 2 },
      { ingredientId: 34, quantity: 1 },
      { ingredientId: 29, quantity: 3 },
    ],
  },
  {
    id: 7,
    name: 'Chocolate cake slice',
    prepMinutes: 6,
    markup: null,
    lines: [
      { ingredientId: 9, quantity: 25 },
      { ingredientId: 12, quantity: 30 },
      { ingredientId: 17, quantity: 8 },
      { ingredientId: 30, quantity: 0.5 },
      { ingredientId: 8, quantity: 15 },
      { ingredientId: 3, quantity: 25 },
      { ingredientId: 31, quantity: 2 },
    ],
  },
  {
    id: 8,
    name: 'Cheesecake slice',
    prepMinutes: 8,
    markup: null,
    lines: [
      { ingredientId: 4, quantity: 60 },
      { ingredientId: 12, quantity: 20 },
      { ingredientId: 30, quantity: 0.5 },
      { ingredientId: 5, quantity: 25 },
      { ingredientId: 24, quantity: 20 },
    ],
  },
  {
    id: 9,
    name: 'Banoffee jar',
    prepMinutes: 10,
    markup: null,
    lines: [
      { ingredientId: 1, quantity: 60 },
      { ingredientId: 5, quantity: 40 },
      { ingredientId: 6, quantity: 10 },
      { ingredientId: 12, quantity: 15 },
    ],
  },
  {
    id: 10,
    name: 'Strawberry tart',
    prepMinutes: 12,
    markup: 90,
    lines: [
      { ingredientId: 9, quantity: 40 },
      { ingredientId: 6, quantity: 25 },
      { ingredientId: 12, quantity: 20 },
      { ingredientId: 30, quantity: 0.5 },
      { ingredientId: 24, quantity: 60 },
      { ingredientId: 5, quantity: 30 },
    ],
  },
  {
    id: 11,
    name: 'Coconut candy box',
    prepMinutes: 25,
    markup: null,
    lines: [
      { ingredientId: 1, quantity: 200 },
      { ingredientId: 23, quantity: 80 },
      { ingredientId: 12, quantity: 40 },
      { ingredientId: 6, quantity: 10 },
    ],
  },
  {
    id: 12,
    name: 'Carrot cake with fudge',
    prepMinutes: 45,
    markup: 80,
    lines: [
      { ingredientId: 9, quantity: 250 },
      { ingredientId: 12, quantity: 300 },
      { ingredientId: 30, quantity: 3 },
      { ingredientId: 8, quantity: 120 },
      { ingredientId: 17, quantity: 40 },
      { ingredientId: 1, quantity: 395 },
      { ingredientId: 6, quantity: 30 },
      { ingredientId: 35, quantity: 1 },
    ],
  },
  {
    id: 13,
    name: 'Lemon pie',
    prepMinutes: 60,
    markup: 90,
    lines: [
      { ingredientId: 1, quantity: 790 },
      { ingredientId: 27, quantity: 4 },
      { ingredientId: 30, quantity: 3 },
      { ingredientId: 12, quantity: 150 },
      { ingredientId: 6, quantity: 60 },
      { ingredientId: 9, quantity: 200 },
      { ingredientId: 35, quantity: 1 },
    ],
  },
  {
    id: 14,
    name: 'Red velvet cake 20 cm',
    prepMinutes: 90,
    markup: 120,
    lines: [
      { ingredientId: 9, quantity: 300 },
      { ingredientId: 12, quantity: 350 },
      { ingredientId: 4, quantity: 300 },
      { ingredientId: 6, quantity: 150 },
      { ingredientId: 30, quantity: 4 },
      { ingredientId: 26, quantity: 15 },
      { ingredientId: 17, quantity: 20 },
      { ingredientId: 3, quantity: 200 },
      { ingredientId: 35, quantity: 1 },
    ],
  },
  {
    id: 15,
    name: 'Wedding cake tier',
    prepMinutes: 180,
    markup: 150,
    lines: [
      { ingredientId: 9, quantity: 600 },
      { ingredientId: 12, quantity: 700 },
      { ingredientId: 6, quantity: 400 },
      { ingredientId: 30, quantity: 8 },
      { ingredientId: 5, quantity: 500 },
      { ingredientId: 20, quantity: 400 },
      { ingredientId: 24, quantity: 300 },
      { ingredientId: 35, quantity: 1 },
    ],
  },
];

@Injectable({ providedIn: 'root' })
export class ProductsService {
  async list(): Promise<Product[]> {
    await delay(250);

    return PRODUCTS;
  }
}

function delay(milliseconds: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, milliseconds));
}
