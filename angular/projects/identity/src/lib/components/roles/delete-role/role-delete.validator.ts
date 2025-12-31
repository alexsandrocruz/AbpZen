import { AbstractControl, ValidationErrors } from '@angular/forms';

export function RoleDeleteValidator() {
  return (control: AbstractControl): ValidationErrors | null => {
    const assignType = control.get('assignType')?.value;
    const role = control.get('role')?.value;

    if (!assignType) {
      return null;
    }

    if (assignType && role) {
      return null;
    }

    return { role: {required: true} };
  };
}
