import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { PaymentRequestWithDetailsDto } from '../volo/payment/requests/models';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  apiName = 'SaasHost';

  createSubscription = (editionId: string, tenantId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaymentRequestWithDetailsDto>(
      {
        method: 'POST',
        url: '/api/saas/subscription',
        params: { editionId, tenantId },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
