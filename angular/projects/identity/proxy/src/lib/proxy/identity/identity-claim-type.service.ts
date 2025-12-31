import type {
  ClaimTypeDto,
  CreateClaimTypeDto,
  GetIdentityClaimTypesInput,
  UpdateClaimTypeDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityClaimTypeService {
  apiName = 'AbpIdentity';

  create = (input: CreateClaimTypeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClaimTypeDto>(
      {
        method: 'POST',
        url: '/api/identity/claim-types',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/claim-types/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClaimTypeDto>(
      {
        method: 'GET',
        url: `/api/identity/claim-types/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetIdentityClaimTypesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ClaimTypeDto>>(
      {
        method: 'GET',
        url: '/api/identity/claim-types',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: UpdateClaimTypeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClaimTypeDto>(
      {
        method: 'PUT',
        url: `/api/identity/claim-types/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
