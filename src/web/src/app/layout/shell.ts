import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-shell',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './shell.html',
  styleUrl: './shell.css',
})
export class Shell {
  private readonly router = inject(Router);

  protected readonly auth = inject(AuthService);

  protected readonly sections = [
    { path: '/ingredients', label: 'Ingredients' },
    { path: '/products', label: 'Products' },
    { path: '/baker', label: 'Baker' },
  ];

  protected async signOut(): Promise<void> {
    this.auth.signOut();

    await this.router.navigate(['/login']);
  }
}
