import { mapEnumToOptions } from '@abp/ng.core';

export enum TenantActivationState {
  Active = 0,
  ActiveWithLimitedTime = 1,
  Passive = 2,
}

export const tenantActivationStateOptions = mapEnumToOptions(TenantActivationState);
