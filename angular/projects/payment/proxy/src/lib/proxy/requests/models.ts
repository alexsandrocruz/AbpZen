import type { PaymentType } from './payment-type.enum';
import type { ExtensibleEntityDto } from '@abp/ng.core';
import type { PaymentRequestState } from './payment-request-state.enum';

export interface PaymentRequestProductDto {
  paymentRequestId?: string;
  code?: string;
  name?: string;
  unitPrice: number;
  count: number;
  totalPrice: number;
  paymentType: PaymentType;
  planId?: string;
  extraProperties: Record<string, object>;
}

export interface PaymentRequestWithDetailsDto extends ExtensibleEntityDto<string> {
  products: PaymentRequestProductDto[];
  currency?: string;
  state: PaymentRequestState;
  failReason?: string;
  emailSendDate?: string;
  gateway?: string;
  externalSubscriptionId?: string;
  totalPrice: number;
  creationTime?: string;
}
