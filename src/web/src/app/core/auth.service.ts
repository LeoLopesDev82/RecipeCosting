import { Injectable, computed, inject, signal } from '@angular/core';
import { ApiClient } from './api-client';

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

interface Session {
  email: string;
  accessToken: string;
  expiresAt: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(ApiClient);
  private readonly emailSignal = signal<string | null>(localStorage.getItem(EMAIL_KEY));

  readonly email = this.emailSignal.asReadonly();
  readonly isSignedIn = computed(() => this.emailSignal() !== null);

  get token(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  async signIn(email: string, password: string): Promise<void> {
    const session = await this.api.post<Session>('/auth/login', { email, password });

    localStorage.setItem(TOKEN_KEY, session.accessToken);
    localStorage.setItem(EMAIL_KEY, session.email);

    this.emailSignal.set(session.email);
  }

  signOut(): void {
    this.emailSignal.set(null);

    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
  }
}
