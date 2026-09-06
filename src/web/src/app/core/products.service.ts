import { Injectable, inject } from '@angular/core';
import { ApiClient } from './api-client';
import { Product, ProductRequest } from './product';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private readonly api = inject(ApiClient);

  list(): Promise<Product[]> {
    return this.api.get<Product[]>('/products');
  }

  create(request: ProductRequest): Promise<Product> {
    return this.api.post<Product>('/products', request);
  }

  update(id: number, request: ProductRequest): Promise<Product> {
    return this.api.put<Product>(`/products/${id}`, request);
  }

  remove(id: number): Promise<void> {
    return this.api.delete(`/products/${id}`);
  }

  preview(request: ProductRequest): Promise<Product> {
    return this.api.post<Product>('/products/preview', request);
  }
}
