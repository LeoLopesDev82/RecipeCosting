# Recipe Costing — web client

The Angular client for the [Recipe Costing API](../../README.md). It shows the ingredients,
the products and the baker's own numbers, and it computes none of them: every figure on
screen was worked out by the API, including the ones that update while a form is typed in.

Angular 22 with standalone components, signals and reactive forms. No component library —
the styling is plain CSS on a small set of tokens in `src/styles.css`.

```bash
npm install
npm start     # http://localhost:4200, expecting the API on https://localhost:7137
npm test
npm run build
```

```
src/app/
├── core/       the HTTP client, authentication, models, validators
├── features/   one folder per screen: login, ingredients, products, baker
└── layout/     the shell: navigation and sign-out
```
