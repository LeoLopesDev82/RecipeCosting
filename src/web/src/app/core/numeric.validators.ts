import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function between(low: number, high: number): ValidatorFn {
  return control => {
    const value = Number(control.value);

    return control.value !== '' && Number.isFinite(value) && value >= low && value <= high
      ? null
      : { between: { low, high } };
  };
}

export function sumBelow(hundred: number, ...names: string[]): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const total = names.reduce((sum, name) => sum + Number(group.get(name)?.value), 0);

    return total < hundred ? null : { sumBelow: hundred };
  };
}
