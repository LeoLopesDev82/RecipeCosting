import { Component, ElementRef, computed, inject, signal, viewChild } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { Decimal } from '../../core/decimal.directive';
import { Ingredient, IngredientRequest, PackageUnit, previewUnitCost } from '../../core/ingredient';
import { IngredientsService } from '../../core/ingredients.service';

const EMPTY = { name: '', packageSize: '', packageUnit: 'g' as PackageUnit, packagePrice: '' };

@Component({
  selector: 'app-ingredients',
  imports: [CurrencyPipe, DecimalPipe, ReactiveFormsModule, Decimal],
  templateUrl: './ingredients.html',
  styleUrl: './ingredients.css',
})
export class Ingredients {
  private readonly ingredients = inject(IngredientsService);
  private readonly editor = viewChild.required<ElementRef<HTMLDialogElement>>('editor');
  private readonly confirmation = viewChild.required<ElementRef<HTMLDialogElement>>('confirmation');

  protected readonly units: PackageUnit[] = ['g', 'ml', 'un'];

  protected readonly loading = signal(true);
  protected readonly saving = signal(false);
  protected readonly failure = signal<string | null>(null);
  protected readonly search = signal('');
  protected readonly editing = signal<Ingredient | null>(null);
  protected readonly doomed = signal<Ingredient | null>(null);

  private readonly rows = signal<Ingredient[]>([]);

  protected readonly form = inject(FormBuilder).nonNullable.group({
    name: ['', Validators.required],
    packageSize: ['', [Validators.required, positive]],
    packageUnit: ['g' as PackageUnit, Validators.required],
    packagePrice: ['', [Validators.required, positive]],
  });

  private readonly draft = toSignal(this.form.valueChanges, { initialValue: EMPTY });

  protected readonly visible = computed(() => {
    const term = this.search().trim().toLowerCase();

    return this.rows().filter(row => row.name.toLowerCase().includes(term));
  });

  protected readonly preview = computed(() => previewUnitCost(this.toRequest({ ...EMPTY, ...this.draft() })));

  constructor() {
    void this.load();
  }

  protected openNew(): void {
    this.editing.set(null);
    this.form.reset(EMPTY);
    this.editor().nativeElement.showModal();
  }

  protected openEdit(ingredient: Ingredient): void {
    this.editing.set(ingredient);
    this.form.setValue({
      name: ingredient.name,
      packageSize: String(ingredient.packageSize),
      packageUnit: ingredient.packageUnit,
      packagePrice: String(ingredient.packagePrice),
    });
    this.editor().nativeElement.showModal();
  }

  protected closeEditor(): void {
    this.editor().nativeElement.close();
  }

  protected async save(): Promise<void> {
    if (this.form.invalid || this.saving()) return;

    const edited = this.editing();
    const request = this.toRequest(this.form.getRawValue());

    await this.attempt(async () => {
      edited
        ? await this.ingredients.update(edited.id, request)
        : await this.ingredients.create(request);

      this.closeEditor();
    });
  }

  protected askToDelete(ingredient: Ingredient): void {
    this.doomed.set(ingredient);
    this.confirmation().nativeElement.showModal();
  }

  protected closeConfirmation(): void {
    this.confirmation().nativeElement.close();
  }

  protected async confirmDelete(): Promise<void> {
    const doomed = this.doomed();

    if (!doomed || this.saving()) return;

    await this.attempt(async () => {
      await this.ingredients.remove(doomed.id);

      this.closeConfirmation();
    });
  }

  // #region Private methods

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

  private toRequest(values: typeof EMPTY): IngredientRequest {
    return {
      name: values.name,
      packageSize: Number(values.packageSize),
      packageUnit: values.packageUnit,
      packagePrice: Number(values.packagePrice),
    };
  }

  private async load(): Promise<void> {
    try {
      this.rows.set(await this.ingredients.list());
    } catch (failure) {
      this.failure.set((failure as Error).message);
    }

    this.loading.set(false);
  }

  // #endregion
}

function positive(control: AbstractControl): Record<string, boolean> | null {
  return Number(control.value) > 0 ? null : { positive: true };
}
