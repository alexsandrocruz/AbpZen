import { mapEnumToOptions } from '@abp/ng.core';

export enum IdentityProPreventConcurrentLoginBehaviour {
  Disabled = 0,
  LogoutFromSameTypeDevices = 1,
  LogoutFromAllDevices = 2,
}

export const identityProPreventConcurrentLoginBehaviourOptions = mapEnumToOptions(
  IdentityProPreventConcurrentLoginBehaviour,
);
