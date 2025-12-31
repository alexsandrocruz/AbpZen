import type {
  AuditLogDto,
  EntityChangeDto,
  EntityChangeFilter,
  EntityChangeWithUsernameDto,
  GetAuditLogListDto,
  GetAverageExecutionDurationPerDayInput,
  GetAverageExecutionDurationPerDayOutput,
  GetEntityChangesDto,
  GetErrorRateFilter,
  GetErrorRateOutput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuditLogsService {
  apiName = 'AbpAuditLogging';

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuditLogDto>(
      {
        method: 'GET',
        url: `/api/audit-logging/audit-logs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAverageExecutionDurationPerDay = (
    filter: GetAverageExecutionDurationPerDayInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, GetAverageExecutionDurationPerDayOutput>(
      {
        method: 'GET',
        url: '/api/audit-logging/audit-logs/statistics/average-execution-duration-per-day',
        params: { startDate: filter.startDate, endDate: filter.endDate },
      },
      { apiName: this.apiName, ...config },
    );

  getEntityChange = (entityChangeId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EntityChangeDto>(
      {
        method: 'GET',
        url: `/api/audit-logging/audit-logs/entity-changes/${entityChangeId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getEntityChangeWithUsername = (entityChangeId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EntityChangeWithUsernameDto>(
      {
        method: 'GET',
        url: `/api/audit-logging/audit-logs/entity-change-with-username/${entityChangeId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getEntityChanges = (input: GetEntityChangesDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<EntityChangeDto>>(
      {
        method: 'GET',
        url: '/api/audit-logging/audit-logs/entity-changes',
        params: {
          auditLogId: input.auditLogId,
          entityChangeType: input.entityChangeType,
          entityId: input.entityId,
          entityTypeFullName: input.entityTypeFullName,
          startDate: input.startDate,
          endDate: input.endDate,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getEntityChangesWithUsername = (input: EntityChangeFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EntityChangeWithUsernameDto[]>(
      {
        method: 'GET',
        url: '/api/audit-logging/audit-logs/entity-changes-with-username',
        params: { entityId: input.entityId, entityTypeFullName: input.entityTypeFullName },
      },
      { apiName: this.apiName, ...config },
    );

  getErrorRate = (filter: GetErrorRateFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetErrorRateOutput>(
      {
        method: 'GET',
        url: '/api/audit-logging/audit-logs/statistics/error-rate',
        params: { startDate: filter.startDate, endDate: filter.endDate },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetAuditLogListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AuditLogDto>>(
      {
        method: 'GET',
        url: '/api/audit-logging/audit-logs',
        params: {
          startTime: input.startTime,
          endTime: input.endTime,
          url: input.url,
          clientId: input.clientId,
          userName: input.userName,
          applicationName: input.applicationName,
          clientIpAddress: input.clientIpAddress,
          correlationId: input.correlationId,
          httpMethod: input.httpMethod,
          httpStatusCode: input.httpStatusCode,
          maxExecutionDuration: input.maxExecutionDuration,
          minExecutionDuration: input.minExecutionDuration,
          hasException: input.hasException,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
