import { AbstractControl, ValidationErrors } from '@angular/forms';

export function EditionDeleteValidator() {
  return (control: AbstractControl): ValidationErrors | null => {
    const assignType = control.get('assignType')?.value;
    const edition = control.get('edition')?.value;

    if (!assignType) {
      return null;
    }

    if (assignType && edition) {
      return null;
    }

    return { edition: { required: true } };
  };
}
