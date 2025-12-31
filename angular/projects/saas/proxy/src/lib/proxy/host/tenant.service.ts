import type {
  EditionLookupDto,
  GetTenantsInput,
  SaasTenantConnectionStringsDto,
  SaasTenantCreateDto,
  SaasTenantDatabasesDto,
  SaasTenantDto,
  SaasTenantSetPasswordDto,
  SaasTenantUpdateDto,
} from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TenantService {
  apiName = 'SaasHost';

  applyDatabaseMigrations = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/saas/tenants/${id}/apply-database-migrations`,
      },
      { apiName: this.apiName, ...config },
    );

  checkConnectionString = (connectionString: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>(
      {
        method: 'GET',
        url: '/api/saas/tenants/check-connection-string',
        params: { connectionString },
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: SaasTenantCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaasTenantDto>(
      {
        method: 'POST',
        url: '/api/saas/tenants',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/saas/tenants/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaasTenantDto>(
      {
        method: 'GET',
        url: `/api/saas/tenants/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getConnectionStrings = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaasTenantConnectionStringsDto>(
      {
        method: 'GET',
        url: `/api/saas/tenants/${id}/connection-strings`,
      },
      { apiName: this.apiName, ...config },
    );

  getDatabases = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaasTenantDatabasesDto>(
      {
        method: 'GET',
        url: '/api/saas/tenants/databases',
      },
      { apiName: this.apiName, ...config },
    );

  getEditionLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, EditionLookupDto[]>(
      {
        method: 'GET',
        url: '/api/saas/tenants/lookup/editions',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetTenantsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaasTenantDto>>(
      {
        method: 'GET',
        url: '/api/saas/tenants',
        params: {
          filter: input.filter,
          getEditionNames: input.getEditionNames,
          editionId: input.editionId,
          expirationDateMin: input.expirationDateMin,
          expirationDateMax: input.expirationDateMax,
          activationState: input.activationState,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  setPassword = (id: string, input: SaasTenantSetPasswordDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/saas/tenants/${id}/set-password`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SaasTenantUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaasTenantDto>(
      {
        method: 'PUT',
        url: `/api/saas/tenants/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateConnectionStrings = (
    id: string,
    input: SaasTenantConnectionStringsDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/saas/tenants/${id}/connection-strings`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
