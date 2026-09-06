import { Injectable, inject } from '@angular/core';
import { ApiClient } from './api-client';
import { Baker, BakerRequest, HourlyCost } from './baker';

@Injectable({ providedIn: 'root' })
export class BakerService {
  private readonly api = inject(ApiClient);

  load(): Promise<Baker> {
    return this.api.get<Baker>('/baker');
  }

  save(request: BakerRequest): Promise<Baker> {
    return this.api.put<Baker>('/baker', request);
  }

  preview(request: BakerRequest): Promise<HourlyCost> {
    return this.api.post<HourlyCost>('/baker/preview', request);
  }
}
