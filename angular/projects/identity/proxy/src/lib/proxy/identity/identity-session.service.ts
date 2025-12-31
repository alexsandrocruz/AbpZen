import type { GetIdentitySessionListInput, IdentitySessionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentitySessionService {
  apiName = 'AbpIdentity';

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentitySessionDto>(
      {
        method: 'GET',
        url: `/api/identity/sessions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetIdentitySessionListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentitySessionDto>>(
      {
        method: 'GET',
        url: '/api/identity/sessions',
        params: {
          userId: input.userId,
          device: input.device,
          clientId: input.clientId,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  revoke = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/sessions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
