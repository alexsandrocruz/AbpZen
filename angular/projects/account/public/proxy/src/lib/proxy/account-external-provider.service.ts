import type { ExternalProviderDto, ExternalProviderItemWithSecretDto, GetByNameInput } from './external-providers/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountExternalProviderService {
  apiName = 'AbpAccountPublic';
  

  getAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExternalProviderDto>({
      method: 'GET',
      url: '/api/account/external-provider',
    },
    { apiName: this.apiName,...config });
  

  getByName = (input: GetByNameInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExternalProviderItemWithSecretDto>({
      method: 'GET',
      url: '/api/account/external-provider/by-name',
      params: { tenantId: input.tenantId, name: input.name },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
