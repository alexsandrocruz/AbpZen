import type {
  IdentityLdapSettingsDto,
  IdentityOAuthSettingsDto,
  IdentitySessionSettingsDto,
  IdentitySettingsDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentitySettingsService {
  apiName = 'AbpIdentity';

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentitySettingsDto>(
      {
        method: 'GET',
        url: '/api/identity/settings',
      },
      { apiName: this.apiName, ...config },
    );

  getLdap = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityLdapSettingsDto>(
      {
        method: 'GET',
        url: '/api/identity/settings/ldap',
      },
      { apiName: this.apiName, ...config },
    );

  getOAuth = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityOAuthSettingsDto>(
      {
        method: 'GET',
        url: '/api/identity/settings/oauth',
      },
      { apiName: this.apiName, ...config },
    );

  getSession = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentitySessionSettingsDto>(
      {
        method: 'GET',
        url: '/api/identity/settings/session',
      },
      { apiName: this.apiName, ...config },
    );

  update = (input: IdentitySettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/identity/settings',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateLdap = (input: IdentityLdapSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/identity/settings/ldap',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateOAuth = (input: IdentityOAuthSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/identity/settings/oauth',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateSession = (input: IdentitySessionSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/identity/settings/session',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
