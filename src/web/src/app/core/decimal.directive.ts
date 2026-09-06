import { Directive, ElementRef, inject } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[appDecimal]',
  host: {
    type: 'text',
    inputmode: 'decimal',
    autocomplete: 'off',
    '(input)': 'sanitise()',
  },
})
export class Decimal {
  private readonly host = inject<ElementRef<HTMLInputElement>>(ElementRef);
  private readonly control = inject(NgControl, { optional: true });

  protected sanitise(): void {
    const input = this.host.nativeElement;
    const typed = input.value;
    const clean = typed
      .replace(/[^\d.]/g, '')
      .replace(/(\.[^.]*)\./g, '$1')
      .replace(/(\.\d{2})\d+/, '$1');
    const caret = (input.selectionStart ?? typed.length) - (typed.length - clean.length);

    input.value = clean;
    input.setSelectionRange(caret, caret);

    this.control?.control?.setValue(clean);
    this.control?.control?.markAsDirty();
  }
}
