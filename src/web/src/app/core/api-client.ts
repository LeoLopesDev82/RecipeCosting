import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, firstValueFrom } from 'rxjs';
import { API_URL } from './api';
import { toApiError } from './api-error';

@Injectable({ providedIn: 'root' })
export class ApiClient {
  private readonly http = inject(HttpClient);
  private readonly inFlight = signal(0);

  readonly pending = computed(() => this.inFlight() > 0);

  get<T>(path: string): Promise<T> {
    return this.send(this.http.get<T>(this.url(path)));
  }

  post<T>(path: string, body: unknown): Promise<T> {
    return this.send(this.http.post<T>(this.url(path), body));
  }

  put<T>(path: string, body: unknown): Promise<T> {
    return this.send(this.http.put<T>(this.url(path), body));
  }

  delete(path: string): Promise<void> {
    return this.send(this.http.delete<void>(this.url(path)));
  }

  private url(path: string): string {
    return `${API_URL}${path}`;
  }

  private async send<T>(request: Observable<T>): Promise<T> {
    this.inFlight.update(count => count + 1);

    try {
      return await firstValueFrom(request);
    } catch (failure) {
      throw toApiError(failure);
    } finally {
      this.inFlight.update(count => count - 1);
    }
  }
}
