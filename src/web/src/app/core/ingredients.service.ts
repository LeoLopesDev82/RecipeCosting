import { Injectable, inject } from '@angular/core';
import { ApiClient } from './api-client';
import { Ingredient, IngredientRequest } from './ingredient';

@Injectable({ providedIn: 'root' })
export class IngredientsService {
  private readonly api = inject(ApiClient);

  list(): Promise<Ingredient[]> {
    return this.api.get<Ingredient[]>('/ingredients');
  }

  create(request: IngredientRequest): Promise<Ingredient> {
    return this.api.post<Ingredient>('/ingredients', request);
  }

  update(id: number, request: IngredientRequest): Promise<Ingredient> {
    return this.api.put<Ingredient>(`/ingredients/${id}`, request);
  }

  remove(id: number): Promise<void> {
    return this.api.delete(`/ingredients/${id}`);
  }
}
