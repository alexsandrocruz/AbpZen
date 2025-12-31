import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';
import { PlanDto } from '@volo/abp.ng.payment/proxy';

export const DEFAULT_PLANS_CREATE_FORM_PROPS = FormProp.createMany<PlanDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'Payment::Name',
    id: 'payment-name',
    validators: () => [Validators.required],
  },
]);

export const DEFAULT_PLANS_EDIT_FORM_PROPS = DEFAULT_PLANS_CREATE_FORM_PROPS;
