import type { GatewayPlanDto, PlanDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PlanService {
  apiName = 'AbpPaymentAdmin';

  getGatewayPlan = (planId: string, gateway: string) =>
    this.restService.request<any, GatewayPlanDto>({
      method: 'GET',
      url: `/api/payment/plans/${planId}/${gateway}`,
    },
    { apiName: this.apiName });

  getPlanList = () =>
    this.restService.request<any, PlanDto[]>({
      method: 'GET',
      url: '/api/payment/plans',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
