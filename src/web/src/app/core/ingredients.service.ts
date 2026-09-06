import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { API_URL } from './api';
import { Ingredient, IngredientRequest } from './ingredient';

@Injectable({ providedIn: 'root' })
export class IngredientsService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${API_URL}/ingredients`;

  list(): Promise<Ingredient[]> {
    return firstValueFrom(this.http.get<Ingredient[]>(this.endpoint));
  }

  create(request: IngredientRequest): Promise<Ingredient> {
    return firstValueFrom(this.http.post<Ingredient>(this.endpoint, request));
  }

  update(id: number, request: IngredientRequest): Promise<Ingredient> {
    return firstValueFrom(this.http.put<Ingredient>(`${this.endpoint}/${id}`, request));
  }

  remove(id: number): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.endpoint}/${id}`));
  }
}
