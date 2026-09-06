import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { API_URL } from './api';

const TOKEN_KEY = 'recipe-costing.token';
const EMAIL_KEY = 'recipe-costing.email';

export interface DemoCredentials {
  email: string;
  password: string;
}

export const DEMO_CREDENTIALS: DemoCredentials = {
  email: 'demo@recipecosting.local',
  password: 'demo1234',
};

interface LoginResponse {
  email: string;
  accessToken: string;
  expiresAt: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly emailSignal = signal<string | null>(localStorage.getItem(EMAIL_KEY));

  readonly email = this.emailSignal.asReadonly();
  readonly isSignedIn = computed(() => this.emailSignal() !== null);

  get token(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  async signIn(email: string, password: string): Promise<boolean> {
    const session = await firstValueFrom(
      this.http.post<LoginResponse>(`${API_URL}/auth/login`, { email, password }),
    );

    localStorage.setItem(TOKEN_KEY, session.accessToken);
    localStorage.setItem(EMAIL_KEY, session.email);

    this.emailSignal.set(session.email);

    return true;
  }

  signOut(): void {
    this.emailSignal.set(null);

    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
  }
}
