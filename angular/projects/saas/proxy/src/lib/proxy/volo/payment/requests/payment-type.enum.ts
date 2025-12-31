import { mapEnumToOptions } from '@abp/ng.core';

export enum PaymentType {
  OneTime = 0,
  Subscription = 1,
}

export const paymentTypeOptions = mapEnumToOptions(PaymentType);
