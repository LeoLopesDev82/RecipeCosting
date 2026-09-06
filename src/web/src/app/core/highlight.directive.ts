import { Directive, ElementRef, effect, inject, input } from '@angular/core';

const CLASS = 'just-saved';
const DURATION = 2000;

@Directive({ selector: '[appHighlight]' })
export class Highlight {
  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef);

  readonly appHighlight = input(false);

  constructor() {
    effect(cleanup => {
      if (!this.appHighlight()) return;

      const element = this.host.nativeElement;

      element.classList.add(CLASS);

      const reveal = setTimeout(() => element.scrollIntoView({ block: 'nearest' }));

      const fade = setTimeout(() => element.classList.remove(CLASS), DURATION);

      cleanup(() => {
        clearTimeout(reveal);
        clearTimeout(fade);
      });
    });
  }
}
