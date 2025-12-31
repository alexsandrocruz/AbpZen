import type {
  EditionCreateDto,
  EditionDto,
  EditionUpdateDto,
  GetEditionsInput,
} from './dtos/models';
import type { GetEditionUsageStatisticsResultDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { PlanDto } from '../volo/payment/plans/models';

@Injectable({
  providedIn: 'root',
})
export class EditionService {
  apiName = 'SaasHost';

  create = (input: EditionCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EditionDto>(
      {
        method: 'POST',
        url: '/api/saas/editions',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/saas/editions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EditionDto>(
      {
        method: 'GET',
        url: `/api/saas/editions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAllList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, EditionDto[]>(
      {
        method: 'GET',
        url: '/api/saas/editions/all',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetEditionsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<EditionDto>>(
      {
        method: 'GET',
        url: '/api/saas/editions',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getPlanLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlanDto[]>(
      {
        method: 'GET',
        url: '/api/saas/editions/plan-lookup',
      },
      { apiName: this.apiName, ...config },
    );

  getUsageStatistics = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetEditionUsageStatisticsResultDto>(
      {
        method: 'GET',
        url: '/api/saas/editions/statistics/usage-statistic',
      },
      { apiName: this.apiName, ...config },
    );

  moveAllTenants = (id: string, editionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/saas/editions/${id}/move-all-tenants`,
        params: { editionId },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: EditionUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EditionDto>(
      {
        method: 'PUT',
        url: `/api/saas/editions/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
