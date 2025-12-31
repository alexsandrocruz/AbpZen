import type {
  GetAvailableRolesInput,
  GetAvailableUsersInput,
  GetIdentityUsersInput,
  GetOrganizationUnitInput,
  IdentityRoleDto,
  IdentityUserDto,
  OrganizationUnitCreateDto,
  OrganizationUnitMoveInput,
  OrganizationUnitRoleInput,
  OrganizationUnitUpdateDto,
  OrganizationUnitUserInput,
  OrganizationUnitWithDetailsDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrganizationUnitService {
  apiName = 'AbpIdentity';

  addMembers = (id: string, input: OrganizationUnitUserInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/organization-units/${id}/members`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  addRoles = (id: string, input: OrganizationUnitRoleInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/organization-units/${id}/roles`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: OrganizationUnitCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrganizationUnitWithDetailsDto>(
      {
        method: 'POST',
        url: '/api/identity/organization-units',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: '/api/identity/organization-units',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrganizationUnitWithDetailsDto>(
      {
        method: 'GET',
        url: `/api/identity/organization-units/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAvailableRoles = (input: GetAvailableRolesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/organization-units/available-roles',
        params: {
          filter: input.filter,
          id: input.id,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getAvailableUsers = (input: GetAvailableUsersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityUserDto>>(
      {
        method: 'GET',
        url: '/api/identity/organization-units/available-users',
        params: {
          filter: input.filter,
          id: input.id,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetOrganizationUnitInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OrganizationUnitWithDetailsDto>>(
      {
        method: 'GET',
        url: '/api/identity/organization-units',
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

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<OrganizationUnitWithDetailsDto>>(
      {
        method: 'GET',
        url: '/api/identity/organization-units/all',
      },
      { apiName: this.apiName, ...config },
    );

  getMembers = (id: string, input: GetIdentityUsersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityUserDto>>(
      {
        method: 'GET',
        url: `/api/identity/organization-units/${id}/members`,
        params: {
          filter: input.filter,
          roleId: input.roleId,
          organizationUnitId: input.organizationUnitId,
          userName: input.userName,
          phoneNumber: input.phoneNumber,
          emailAddress: input.emailAddress,
          name: input.name,
          surname: input.surname,
          isLockedOut: input.isLockedOut,
          notActive: input.notActive,
          emailConfirmed: input.emailConfirmed,
          isExternal: input.isExternal,
          maxCreationTime: input.maxCreationTime,
          minCreationTime: input.minCreationTime,
          maxModifitionTime: input.maxModifitionTime,
          minModifitionTime: input.minModifitionTime,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          extraProperties: input.extraProperties,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getRoles = (id: string, input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: `/api/identity/organization-units/${id}/roles`,
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  move = (id: string, input: OrganizationUnitMoveInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/organization-units/${id}/move`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  moveAllUsers = (id: string, organizationId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/organization-units/${id}/move-all-users`,
        params: { organizationId },
      },
      { apiName: this.apiName, ...config },
    );

  removeMember = (id: string, memberId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/organization-units/${id}/members/${memberId}`,
      },
      { apiName: this.apiName, ...config },
    );

  removeRole = (id: string, roleId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/organization-units/${id}/roles/${roleId}`,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: OrganizationUnitUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrganizationUnitWithDetailsDto>(
      {
        method: 'PUT',
        url: `/api/identity/organization-units/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
