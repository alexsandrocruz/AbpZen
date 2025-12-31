import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { defaultUriPattern } from '../defaults/default-uri-pattern';
import { NewLineRegex } from './new-line-regex';

export function UrisValidator(): ValidatorFn {
  const pattern = defaultUriPattern;
  return (control: AbstractControl): ValidationErrors | null => {
    if (isEmptyInputValue(control.value)) {
      return null;
    }
    const value: string = control.value;
    const rows = value.split(NewLineRegex).filter(item => !!item.trim());
    if (rows.length === 0) {
      return null;
    }
    const isAllRowsValid = rows.every(item => pattern.test(item));
    if (isAllRowsValid) {
      return null;
    }
    return { url: pattern.toString(), actualValue: value };
  };
}

function isEmptyInputValue(value: unknown): boolean {
  return (
    value == null || ((typeof value === 'string' || Array.isArray(value)) && value.length === 0)
  );
}
