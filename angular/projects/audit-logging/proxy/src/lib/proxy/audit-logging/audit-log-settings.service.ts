import type { AuditLogGlobalSettingsDto, AuditLogSettingsDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuditLogSettingsService {
  apiName = 'AbpAuditLogging';

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuditLogSettingsDto>(
      {
        method: 'GET',
        url: '/api/audit-logging/settings',
      },
      { apiName: this.apiName, ...config },
    );

  getGlobal = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuditLogGlobalSettingsDto>(
      {
        method: 'GET',
        url: '/api/audit-logging/settings/global',
      },
      { apiName: this.apiName, ...config },
    );

  update = (input: AuditLogSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/audit-logging/settings',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateGlobal = (input: AuditLogGlobalSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/audit-logging/settings/global',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
