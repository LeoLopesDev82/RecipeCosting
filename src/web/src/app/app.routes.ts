import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    title: 'Sign in',
    loadComponent: () => import('./features/login/login').then(m => m.Login),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layout/shell').then(m => m.Shell),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'ingredients' },
      {
        path: 'ingredients',
        title: 'Ingredients',
        loadComponent: () => import('./features/ingredients/ingredients').then(m => m.Ingredients),
      },
      {
        path: 'products',
        title: 'Products',
        loadComponent: () => import('./features/products/products').then(m => m.Products),
      },
      {
        path: 'baker',
        title: 'Baker',
        loadComponent: () => import('./features/baker/baker').then(m => m.Baker),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
