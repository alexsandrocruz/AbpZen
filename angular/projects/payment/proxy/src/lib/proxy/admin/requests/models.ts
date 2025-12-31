import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { PaymentType } from '../../requests/payment-type.enum';
import type { PaymentRequestState } from '../../requests/payment-request-state.enum';

export interface PaymentRequestGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  creationDateMax?: string;
  creationDateMin?: string;
  paymentType?: PaymentType;
  status?: PaymentRequestState;
}
