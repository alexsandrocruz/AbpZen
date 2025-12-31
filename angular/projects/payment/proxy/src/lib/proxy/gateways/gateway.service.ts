import type { GatewayDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GatewayService {
  apiName = 'AbpPaymentAdmin';

  getGatewayConfiguration = () =>
    this.restService.request<any, GatewayDto[]>({
      method: 'GET',
      url: '/api/payment/gateways',
    },
    { apiName: this.apiName });

  getSubscriptionSupportedGateways = () =>
    this.restService.request<any, GatewayDto[]>({
      method: 'GET',
      url: '/api/payment/gateways/subscription-supported',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
