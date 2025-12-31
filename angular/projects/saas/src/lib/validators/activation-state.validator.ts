import { Provider } from '@angular/core';
import { AsyncValidatorFn, FormGroup } from '@angular/forms';
import { of } from 'rxjs';
import { TenantActivationState } from '@volo/abp.ng.saas/proxy';
import { TENANT_FORM_ASYNC_VALIDATORS_TOKEN } from '../tokens/tenant-form-validators.token';

export const TENANT_ACTIVATION_STATE_VALIDATOR_PROVIDER: Provider = {
  provide: TENANT_FORM_ASYNC_VALIDATORS_TOKEN,
  multi: true,
  useFactory: tenantActivationStateValidator,
};

//Note: taken from https://stackoverflow.com/a/51094336/9169960, maybe it can be improved!
export function tenantActivationStateValidator(): AsyncValidatorFn {
  const validate = (group: FormGroup) => {
    const activationState = group?.get('activationState');
    const activationEndDate = group?.get('activationEndDate');

    if (!activationState || !activationEndDate) {
      return of(null);
    }

    if (
      activationState.value === TenantActivationState.ActiveWithLimitedTime &&
      !activationEndDate.value
    ) {
      activationEndDate.setErrors({ required: true });
    }

    return of(null);
  };

  return validate;
}
