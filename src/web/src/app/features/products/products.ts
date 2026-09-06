import { Component, ElementRef, computed, inject, signal, viewChild } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { BakerSettings } from '../../core/baker';
import { BakerService } from '../../core/baker.service';
import { Decimal } from '../../core/decimal.directive';
import { Ingredient } from '../../core/ingredient';
import { IngredientsService } from '../../core/ingredients.service';
import { Pantry, Product, lineCostOf, productCostOf } from '../../core/product';
import { ProductsService } from '../../core/products.service';

const NO_SETTINGS: BakerSettings = {
  monthlyIncome: 0,
  hoursPerDay: 0,
  daysPerWeek: 0,
  monthlyFixedCosts: 0,
  defaultMarkup: 0,
  cardFee: 0,
  tax: 0,
};

const EMPTY = { name: '', prepMinutes: '', markup: '', lines: [] as LineValue[] };

interface LineValue {
  ingredientId: string;
  quantity: string;
}

@Component({
  selector: 'app-products',
  imports: [CurrencyPipe, DecimalPipe, ReactiveFormsModule, Decimal],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products {
  private readonly products = inject(ProductsService);
  private readonly ingredients = inject(IngredientsService);
  private readonly baker = inject(BakerService);
  private readonly builder = inject(FormBuilder);
  private readonly editor = viewChild.required<ElementRef<HTMLDialogElement>>('editor');
  private readonly confirmation = viewChild.required<ElementRef<HTMLDialogElement>>('confirmation');

  protected readonly loading = signal(true);
  protected readonly search = signal('');
  protected readonly editing = signal<Product | null>(null);
  protected readonly doomed = signal<Product | null>(null);
  protected readonly shelf = signal<Ingredient[]>([]);

  private readonly rows = signal<Product[]>([]);
  private readonly settings = signal<BakerSettings>(NO_SETTINGS);

  private readonly pantry = computed<Pantry>(
    () => new Map(this.shelf().map(ingredient => [ingredient.id, ingredient])),
  );

  protected readonly form = this.builder.nonNullable.group({
    name: ['', Validators.required],
    prepMinutes: ['', Validators.required],
    markup: [''],
    lines: this.builder.array([this.lineGroup()]),
  });

  private readonly draft = toSignal(this.form.valueChanges, { initialValue: EMPTY });

  protected readonly visible = computed(() => {
    const term = this.search().trim().toLowerCase();
    const settings = this.settings();
    const pantry = this.pantry();

    return this.rows()
      .filter(row => row.name.toLowerCase().includes(term))
      .map(row => ({ product: row, cost: productCostOf(row, settings, pantry) }));
  });

  protected readonly draftLines = computed(() => {
    const pantry = this.pantry();

    return (this.draft().lines ?? []).map(line => {
      const ingredient = pantry.get(Number(line.ingredientId));

      return {
        unit: ingredient?.packageUnit ?? '',
        cost: lineCostOf(
          { ingredientId: Number(line.ingredientId), quantity: Number(line.quantity) },
          pantry,
        ),
      };
    });
  });

  protected readonly draftCost = computed(() => {
    const draft = { ...EMPTY, ...this.draft() };

    return productCostOf(
      {
        id: 0,
        name: draft.name,
        prepMinutes: Number(draft.prepMinutes),
        markup: draft.markup === '' ? null : Number(draft.markup),
        lines: (draft.lines ?? []).map(line => ({
          ingredientId: Number(line.ingredientId),
          quantity: Number(line.quantity),
        })),
      },
      this.settings(),
      this.pantry(),
    );
  });

  constructor() {
    void this.load();
  }

  protected get lines(): FormArray {
    return this.form.controls.lines;
  }

  protected openNew(): void {
    this.editing.set(null);
    this.form.reset({ name: '', prepMinutes: '', markup: '' });
    this.lines.clear();
    this.lines.push(this.lineGroup());
    this.editor().nativeElement.showModal();
  }

  protected openEdit(product: Product): void {
    this.editing.set(product);
    this.form.reset({
      name: product.name,
      prepMinutes: String(product.prepMinutes),
      markup: product.markup === null ? '' : String(product.markup),
    });
    this.lines.clear();
    product.lines.forEach(line =>
      this.lines.push(this.lineGroup(String(line.ingredientId), String(line.quantity))),
    );
    this.editor().nativeElement.showModal();
  }

  protected addLine(): void {
    this.lines.push(this.lineGroup());
  }

  protected removeLine(index: number): void {
    this.lines.removeAt(index);
  }

  protected closeEditor(): void {
    this.editor().nativeElement.close();
  }

  protected askToDelete(product: Product): void {
    this.doomed.set(product);
    this.confirmation().nativeElement.showModal();
  }

  protected closeConfirmation(): void {
    this.confirmation().nativeElement.close();
  }

  private lineGroup(ingredientId = '', quantity = '') {
    return this.builder.nonNullable.group({ ingredientId: [ingredientId], quantity: [quantity] });
  }

  private async load(): Promise<void> {
    const [products, shelf, settings] = await Promise.all([
      this.products.list(),
      this.ingredients.list(),
      this.baker.load(),
    ]);

    this.rows.set(products);
    this.shelf.set(shelf);
    this.settings.set(settings);
    this.loading.set(false);
  }
}
