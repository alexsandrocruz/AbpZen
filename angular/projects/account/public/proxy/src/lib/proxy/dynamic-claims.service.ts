import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DynamicClaimsService {
  apiName = 'AbpAccountPublic';

  refresh = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/dynamic-claims/refresh',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
