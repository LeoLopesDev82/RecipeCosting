import { Component, ElementRef, computed, inject, signal, viewChild } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, debounceTime, from, map, of, switchMap, tap } from 'rxjs';
import { Ingredient } from '../../core/ingredient';
import { IngredientsService } from '../../core/ingredients.service';
import { Decimal } from '../../core/decimal.directive';
import { NO_COST, Product, ProductLine, ProductRequest } from '../../core/product';
import { ProductsService } from '../../core/products.service';
import { between, distinctBy, optionalBetween } from '../../core/numeric.validators';

const INVALID = 'This recipe cannot be priced yet. The figures below are the last ones that could.';

interface Preview {
  priced: Product | null;
  problem: string | null;
}

const NOTHING_YET: Preview = { priced: null, problem: null };

@Component({
  selector: 'app-products',
  imports: [CurrencyPipe, DecimalPipe, ReactiveFormsModule, Decimal],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products {
  private readonly products = inject(ProductsService);
  private readonly ingredients = inject(IngredientsService);
  private readonly builder = inject(FormBuilder);
  private readonly editor = viewChild.required<ElementRef<HTMLDialogElement>>('editor');
  private readonly confirmation = viewChild.required<ElementRef<HTMLDialogElement>>('confirmation');

  protected readonly loading = signal(true);
  protected readonly saving = signal(false);
  protected readonly failure = signal<string | null>(null);
  protected readonly search = signal('');
  protected readonly editing = signal<Product | null>(null);
  protected readonly doomed = signal<Product | null>(null);
  protected readonly shelf = signal<Ingredient[]>([]);

  private readonly rows = signal<Product[]>([]);
  private readonly lastGood = signal<Product | null>(null);

  protected readonly form = this.builder.nonNullable.group({
    name: ['', Validators.required],
    prepMinutes: ['', between(0, 10_000)],
    markup: ['', optionalBetween(0, 1000)],
    lines: this.builder.array([this.lineGroup()], distinctBy('ingredientId')),
  });

  private readonly preview = toSignal(
    this.form.valueChanges.pipe(
      debounceTime(200),
      switchMap(() => (this.form.invalid ? of(refused()) : this.ask())),
    ),
    { initialValue: NOTHING_YET },
  );

  protected readonly visible = computed(() => {
    const term = this.search().trim().toLowerCase();

    return this.rows().filter(row => row.name.toLowerCase().includes(term));
  });

  protected readonly priced = computed(
    () => this.preview().priced ?? this.lastGood() ?? this.editing(),
  );

  protected readonly cost = computed(() => this.priced()?.cost ?? NO_COST);
  protected readonly problem = computed(() => this.preview().problem);

  constructor() {
    void this.load();
  }

  protected get lines(): FormArray {
    return this.form.controls.lines;
  }

  protected lineCost(line: AbstractControl): number {
    return this.pricedLine(line)?.cost ?? 0;
  }

  protected lineUnit(line: AbstractControl): string {
    return this.pricedLine(line)?.unit ?? '—';
  }

  protected openNew(): void {
    this.editing.set(null);
    this.lastGood.set(null);
    this.form.reset({ name: '', prepMinutes: '', markup: '' });
    this.lines.clear();
    this.lines.push(this.lineGroup());
    this.editor().nativeElement.showModal();
  }

  protected openEdit(product: Product): void {
    this.editing.set(product);
    this.lastGood.set(product);
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

  protected removeLine(line: AbstractControl): void {
    this.lines.removeAt(this.lines.controls.indexOf(line));
  }

  protected closeEditor(): void {
    this.editor().nativeElement.close();
  }

  protected async save(): Promise<void> {
    if (this.form.invalid || this.saving()) return;

    const edited = this.editing();
    const request = this.toRequest();

    await this.attempt(async () => {
      edited
        ? await this.products.update(edited.id, request)
        : await this.products.create(request);

      this.closeEditor();
    });
  }

  protected askToDelete(product: Product): void {
    this.doomed.set(product);
    this.confirmation().nativeElement.showModal();
  }

  protected closeConfirmation(): void {
    this.confirmation().nativeElement.close();
  }

  protected async confirmDelete(): Promise<void> {
    const doomed = this.doomed();

    if (!doomed || this.saving()) return;

    await this.attempt(async () => {
      await this.products.remove(doomed.id);

      this.closeConfirmation();
    });
  }

  // #region Private methods

  private pricedLine(line: AbstractControl): ProductLine | undefined {
    const ingredientId = Number(line.value.ingredientId);

    return this.priced()?.lines.find(priced => priced.ingredientId === ingredientId);
  }

  private ask() {
    return from(this.products.preview(this.toRequest())).pipe(
      tap(priced => this.lastGood.set(priced)),
      map(priced => ({ priced, problem: null })),
      catchError((failure: Error) => of({ priced: null, problem: failure.message })),
    );
  }

  private lineGroup(ingredientId = '', quantity = '') {
    return this.builder.nonNullable.group({
      ingredientId: [ingredientId, Validators.required],
      quantity: [quantity, between(0.001, 1_000_000)],
    });
  }

  private toRequest(): ProductRequest {
    const values = this.form.getRawValue();

    return {
      name: values.name,
      prepMinutes: Number(values.prepMinutes),
      markup: values.markup === '' ? null : Number(values.markup),
      lines: values.lines.map(line => ({
        ingredientId: Number(line.ingredientId),
        quantity: Number(line.quantity),
      })),
    };
  }

  private async attempt(work: () => Promise<void>): Promise<void> {
    this.saving.set(true);
    this.failure.set(null);

    try {
      await work();
      await this.load();
    } catch (failure) {
      this.failure.set((failure as Error).message);
    }

    this.saving.set(false);
  }

  private async load(): Promise<void> {
    try {
      const [products, shelf] = await Promise.all([
        this.products.list(),
        this.ingredients.list(),
      ]);

      this.rows.set(products);
      this.shelf.set(shelf);
    } catch (failure) {
      this.failure.set((failure as Error).message);
    }

    this.loading.set(false);
  }

  // #endregion
}

function refused(): Preview {
  return { priced: null, problem: INVALID };
}
