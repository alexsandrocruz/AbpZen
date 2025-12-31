import type {
  ClaimTypeDto,
  DownloadTokenResultDto,
  ExternalLoginProviderDto,
  GetIdentityUserListAsFileInput,
  GetIdentityUsersInput,
  GetImportInvalidUsersFileInput,
  GetImportUsersSampleFileInput,
  IdentityRoleDto,
  IdentityRoleLookupDto,
  IdentityUserClaimDto,
  IdentityUserCreateDto,
  IdentityUserDto,
  IdentityUserUpdateDto,
  IdentityUserUpdatePasswordInput,
  IdentityUserUpdateRolesDto,
  ImportExternalUserInput,
  ImportUsersFromFileInputWithStream,
  ImportUsersFromFileOutput,
  OrganizationUnitDto,
  OrganizationUnitLookupDto,
  OrganizationUnitWithDetailsDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityUserService {
  apiName = 'AbpIdentity';

  create = (input: IdentityUserCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'POST',
        url: '/api/identity/users',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/users/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  findByEmail = (email: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'GET',
        url: `/api/identity/users/by-email/${email}`,
      },
      { apiName: this.apiName, ...config },
    );

  findById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'GET',
        url: `/api/identity/users/by-id/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  findByUsername = (username: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'GET',
        url: `/api/identity/users/by-username/${username}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'GET',
        url: `/api/identity/users/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAllClaimTypes = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClaimTypeDto[]>(
      {
        method: 'GET',
        url: '/api/identity/users/all-claim-types',
      },
      { apiName: this.apiName, ...config },
    );

  getAssignableRoles = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/users/assignable-roles',
      },
      { apiName: this.apiName, ...config },
    );

  getAvailableOrganizationUnits = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<OrganizationUnitWithDetailsDto>>(
      {
        method: 'GET',
        url: '/api/identity/users/available-organization-units',
      },
      { apiName: this.apiName, ...config },
    );

  getClaims = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserClaimDto[]>(
      {
        method: 'GET',
        url: `/api/identity/users/${id}/claims`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/identity/users/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getExternalLoginProviders = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExternalLoginProviderDto[]>(
      {
        method: 'GET',
        url: '/api/identity/users/external-login-Providers',
      },
      { apiName: this.apiName, ...config },
    );

  getImportInvalidUsersFile = (
    input: GetImportInvalidUsersFileInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/identity/users/download-import-invalid-users-file',
        params: { token: input.token },
      },
      { apiName: this.apiName, ...config },
    );

  getImportUsersSampleFile = (
    input: GetImportUsersSampleFileInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/identity/users/import-users-sample-file',
        params: { fileType: input.fileType, token: input.token },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetIdentityUsersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentityUserDto>>(
      {
        method: 'GET',
        url: '/api/identity/users',
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

  getListAsCsvFile = (input: GetIdentityUserListAsFileInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/identity/users/export-as-csv',
        params: {
          token: input.token,
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

  getListAsExcelFile = (input: GetIdentityUserListAsFileInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/identity/users/export-as-excel',
        params: {
          token: input.token,
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

  getOrganizationUnitLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrganizationUnitLookupDto[]>(
      {
        method: 'GET',
        url: '/api/identity/users/lookup/organization-units',
      },
      { apiName: this.apiName, ...config },
    );

  getOrganizationUnits = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrganizationUnitDto[]>(
      {
        method: 'GET',
        url: `/api/identity/users/${id}/organization-units`,
      },
      { apiName: this.apiName, ...config },
    );

  getRoleLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityRoleLookupDto[]>(
      {
        method: 'GET',
        url: '/api/identity/users/lookup/roles',
      },
      { apiName: this.apiName, ...config },
    );

  getRoles = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: `/api/identity/users/${id}/roles`,
      },
      { apiName: this.apiName, ...config },
    );

  getTwoFactorEnabled = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>(
      {
        method: 'GET',
        url: `/api/identity/users/${id}/two-factor-enabled`,
      },
      { apiName: this.apiName, ...config },
    );

  importExternalUser = (input: ImportExternalUserInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'POST',
        url: '/api/identity/users/import-external-user',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  importUsersFromFile = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ImportUsersFromFileOutput>(
      {
        method: 'POST',
        url: '/api/identity/users/import-users-from-file',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lock = (id: string, lockoutEnd: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/lock/${lockoutEnd}`,
      },
      { apiName: this.apiName, ...config },
    );

  setTwoFactorEnabled = (id: string, enabled: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/two-factor/${enabled}`,
      },
      { apiName: this.apiName, ...config },
    );

  unlock = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/unlock`,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: IdentityUserUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateClaims = (id: string, input: IdentityUserClaimDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/claims`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updatePassword = (
    id: string,
    input: IdentityUserUpdatePasswordInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/change-password`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateRoles = (id: string, input: IdentityUserUpdateRolesDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/identity/users/${id}/roles`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
