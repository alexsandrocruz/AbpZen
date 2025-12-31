import type { PaymentRequestGetListInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { PaymentRequestWithDetailsDto } from '../../requests/models';

@Injectable({
  providedIn: 'root',
})
export class PaymentRequestAdminService {
  apiName = 'AbpPaymentAdmin';

  get = (id: string) =>
    this.restService.request<any, PaymentRequestWithDetailsDto>({
      method: 'GET',
      url: `/api/payment-admin/payment-requests/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: PaymentRequestGetListInput) =>
    this.restService.request<any, PagedResultDto<PaymentRequestWithDetailsDto>>({
      method: 'GET',
      url: '/api/payment-admin/payment-requests',
      params: { filter: input.filter, creationDateMax: input.creationDateMax, creationDateMin: input.creationDateMin, paymentType: input.paymentType, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
