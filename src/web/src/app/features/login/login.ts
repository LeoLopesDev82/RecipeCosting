import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, DEMO_CREDENTIALS } from '../../core/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly demo = DEMO_CREDENTIALS;
  protected readonly busy = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly form = inject(FormBuilder).nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  protected fillDemo(): void {
    this.form.setValue({ email: this.demo.email, password: this.demo.password });
  }

  protected async submit(): Promise<void> {
    if (this.form.invalid || this.busy()) return;

    this.busy.set(true);
    this.error.set(null);

    const { email, password } = this.form.getRawValue();

    try {
      await this.auth.signIn(email, password);

      await this.router.navigate(['/ingredients']);
    } catch (failure) {
      this.error.set((failure as Error).message);
    }

    this.busy.set(false);
  }
}
