import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Decimal } from './decimal.directive';

@Component({
  imports: [ReactiveFormsModule, Decimal],
  template: `<input appDecimal [formControl]="price" />`,
})
class Host {
  readonly price = new FormControl('', { nonNullable: true });
}

describe('Decimal', () => {
  let fixture: ComponentFixture<Host>;
  let input: HTMLInputElement;

  const type = (text: string) => {
    input.value = text;
    input.dispatchEvent(new Event('input'));
    fixture.detectChanges();
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [Host] }).compileComponents();

    fixture = TestBed.createComponent(Host);
    fixture.detectChanges();

    input = fixture.nativeElement.querySelector('input');
  });

  it('keeps digits and a single dot', () => {
    type('12.5');

    expect(input.value).toBe('12.5');
    expect(fixture.componentInstance.price.value).toBe('12.5');
  });

  it('drops the comma, because the decimal separator is a dot', () => {
    type('12,5');

    expect(input.value).toBe('125');
  });

  it('drops anything that is not a digit', () => {
    type('12a5x');

    expect(input.value).toBe('125');
  });

  it('keeps only the first dot', () => {
    type('1.2.5');

    expect(input.value).toBe('1.25');
  });

  it('cuts what goes past two decimal places', () => {
    type('18.98765');

    expect(input.value).toBe('18.98');
  });

  it('leaves the whole part alone', () => {
    type('1000000.99');

    expect(input.value).toBe('1000000.99');
  });

  it('marks the field as edited, which writing to the control does not do on its own', () => {
    expect(fixture.componentInstance.price.pristine).toBe(true);

    type('7');

    expect(fixture.componentInstance.price.dirty).toBe(true);
  });
});
