import type { GetAccountIdentitySessionListInput } from './models';
import type { IdentitySessionDto } from './volo/abp/identity/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountSessionService {
  apiName = 'AbpAccountPublic';

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentitySessionDto>(
      {
        method: 'GET',
        url: `/api/account/sessions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetAccountIdentitySessionListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentitySessionDto>>(
      {
        method: 'GET',
        url: '/api/account/sessions',
        params: {
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
        url: `/api/account/sessions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
