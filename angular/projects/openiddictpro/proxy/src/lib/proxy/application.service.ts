import type {
  ApplicationDto,
  ApplicationTokenLifetimeDto,
  CreateApplicationInput,
  GetApplicationListInput,
  UpdateApplicationInput,
} from './applications/dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApplicationService {
  apiName = 'OpenIddictPro';

  create = (input: CreateApplicationInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationDto>(
      {
        method: 'POST',
        url: '/api/openiddict/applications',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: '/api/openiddict/applications',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationDto>(
      {
        method: 'GET',
        url: `/api/openiddict/applications/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetApplicationListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ApplicationDto>>(
      {
        method: 'GET',
        url: '/api/openiddict/applications',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getTokenLifetime = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationTokenLifetimeDto>(
      {
        method: 'GET',
        url: `/api/openiddict/applications/${id}/token-lifetime`,
      },
      { apiName: this.apiName, ...config },
    );

  setTokenLifetime = (
    id: string,
    input: ApplicationTokenLifetimeDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/openiddict/applications/${id}/token-lifetime`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: UpdateApplicationInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationDto>(
      {
        method: 'PUT',
        url: `/api/openiddict/applications/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
