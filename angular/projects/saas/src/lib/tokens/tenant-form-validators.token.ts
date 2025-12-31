import { InjectionToken } from '@angular/core';
import { AsyncValidatorFn, ValidatorFn } from '@angular/forms';

export const TENANT_FORM_ASYNC_VALIDATORS_TOKEN = new InjectionToken<
  AsyncValidatorFn[] | undefined
>(undefined);

export const TENANT_FORM_VALIDATORS_TOKEN = new InjectionToken<ValidatorFn[] | undefined>(
  undefined,
);
