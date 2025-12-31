import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityExternalLoginService {
  apiName = 'AbpIdentity';

  createOrUpdate = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/identity/external-login',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
