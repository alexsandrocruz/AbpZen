import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function OrganizationUnitDeleteValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const assignType = control.get('assignType')?.value;
    const organizationUnitId = control.get('organizationUnitId')?.value;

    if (!assignType) {
      return null;
    }

    if (assignType && organizationUnitId) {
      return null;
    }

    return { organizationUnitId: { required: true } };
  };
}
