import type { GetIdentitySecurityLogListInput, IdentitySecurityLogDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentitySecurityLogService {
  apiName = 'AbpIdentity';

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentitySecurityLogDto>(
      {
        method: 'GET',
        url: `/api/identity/security-logs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetIdentitySecurityLogListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentitySecurityLogDto>>(
      {
        method: 'GET',
        url: '/api/identity/security-logs',
        params: {
          startTime: input.startTime,
          endTime: input.endTime,
          applicationName: input.applicationName,
          identity: input.identity,
          action: input.action,
          userName: input.userName,
          clientId: input.clientId,
          correlationId: input.correlationId,
          clientIpAddress: input.clientIpAddress,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
