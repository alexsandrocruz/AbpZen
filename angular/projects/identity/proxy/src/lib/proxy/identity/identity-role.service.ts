import type {
  ClaimTypeDto,
  GetIdentityRoleListInput,
  IdentityRoleClaimDto,
  IdentityRoleCreateDto,
  IdentityRoleDto,
  IdentityRoleUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityRoleService {
  apiName = 'AbpIdentity';

  create = (input: IdentityRoleCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'POST',
        url: '/api/identity/roles',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/roles/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'GET',
        url: `/api/identity/roles/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAllClaimTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClaimTypeDto[]>(
      {
        method: 'GET',
        url: '/api/identity/roles/all-claim-types',
      },
      { apiName: this.apiName, ...config },
    );

  getAllList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/roles/all',
      },
      { apiName: this.apiName, ...config },
    );

  getClaims = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityRoleClaimDto[]>(
      {
        method: 'GET',
        url: `/api/identity/roles/${id}/claims`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetIdentityRoleListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/roles',
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

  moveAllUsers = (id: string, roleId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/roles/${id}/move-all-users`,
        params: { roleId },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: IdentityRoleUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'PUT',
        url: `/api/identity/roles/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateClaims = (id: string, input: IdentityRoleClaimDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/roles/${id}/claims`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
