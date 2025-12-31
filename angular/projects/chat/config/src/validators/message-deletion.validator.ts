import { FormGroup, ValidatorFn } from '@angular/forms';

export function messageDeletionValidator(): ValidatorFn {
  const validate = (formGroup: FormGroup) => {
    const deletingMessagesControl = formGroup.get('deletingMessages');
    const messageDeletionPeriodControl = formGroup.get('messageDeletionPeriod');

    if (deletingMessagesControl?.value !== 3) {
      return null;
    }

    if (deletingMessagesControl?.value === 3 && messageDeletionPeriodControl?.value !== null) {
      return null;
    }

    messageDeletionPeriodControl?.setErrors({ required: true });
    return null;
  };

  return validate;
}
