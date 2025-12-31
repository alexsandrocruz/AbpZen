import { FormControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function timeRangeValidator([controlName1, controlName2]: string[]): ValidatorFn {
  return (control: FormControl): ValidationErrors | null => {
    const startTime = control.value[controlName1];
    const endTime = control.value[controlName2];

    if (!startTime && !endTime) {
      return { required: true };
    }

    if (!startTime || !endTime) {
      return { invalid: true };
    }

    return null;
  };
}
