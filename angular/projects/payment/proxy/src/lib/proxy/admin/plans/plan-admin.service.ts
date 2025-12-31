import type { GatewayPlanCreateInput, GatewayPlanGetListInput, GatewayPlanUpdateInput, PlanCreateInput, PlanGetListInput, PlanUpdateInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GatewayPlanDto, PlanDto } from '../../plans/models';

@Injectable({
  providedIn: 'root',
})
export class PlanAdminService {
  apiName = 'AbpPaymentAdmin';

  create = (input: PlanCreateInput) =>
    this.restService.request<any, PlanDto>({
      method: 'POST',
      url: '/api/payment-admin/plans',
      body: input,
    },
    { apiName: this.apiName });

  createGatewayPlan = (planId: string, input: GatewayPlanCreateInput) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/payment-admin/plans/${planId}/external-plans`,
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/payment-admin/plans',
      params: { id },
    },
    { apiName: this.apiName });

  deleteGatewayPlan = (planId: string, gateway: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/payment-admin/plans/${planId}/external-plans/${gateway}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, PlanDto>({
      method: 'GET',
      url: `/api/payment-admin/plans/${id}`,
    },
    { apiName: this.apiName });

  getGatewayPlans = (planId: string, input: GatewayPlanGetListInput) =>
    this.restService.request<any, PagedResultDto<GatewayPlanDto>>({
      method: 'GET',
      url: `/api/payment-admin/plans/${planId}/external-plans`,
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  getList = (input: PlanGetListInput) =>
    this.restService.request<any, PagedResultDto<PlanDto>>({
      method: 'GET',
      url: '/api/payment-admin/plans',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  update = (id: string, input: PlanUpdateInput) =>
    this.restService.request<any, PlanDto>({
      method: 'PUT',
      url: `/api/payment-admin/plans/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  updateGatewayPlan = (planId: string, gateway: string, input: GatewayPlanUpdateInput) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/payment-admin/plans/${planId}/external-plans/${gateway}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
