import type { EntityDto, ExtensibleEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { EntityChangeType } from '../auditing/entity-change-type.enum';

export interface AuditLogActionDto extends ExtensibleEntityDto<string> {
  tenantId?: string;
  auditLogId?: string;
  serviceName?: string;
  methodName?: string;
  parameters?: string;
  executionTime?: string;
  executionDuration: number;
}

export interface AuditLogDto extends ExtensibleEntityDto<string> {
  userId?: string;
  userName?: string;
  tenantId?: string;
  tenantName?: string;
  impersonatorUserId?: string;
  impersonatorUserName?: string;
  impersonatorTenantId?: string;
  impersonatorTenantName?: string;
  executionTime?: string;
  executionDuration: number;
  clientIpAddress?: string;
  clientId?: string;
  clientName?: string;
  browserInfo?: string;
  httpMethod?: string;
  url?: string;
  exceptions?: string;
  comments?: string;
  httpStatusCode?: number;
  applicationName?: string;
  correlationId?: string;
  entityChanges: EntityChangeDto[];
  actions: AuditLogActionDto[];
}

export interface AuditLogGlobalSettingsDto extends AuditLogSettingsDto {
  isPeriodicDeleterEnabled: boolean;
}

export interface AuditLogSettingsDto {
  isExpiredDeleterEnabled: boolean;
  expiredDeleterPeriod: number;
}

export interface EntityChangeDto extends ExtensibleEntityDto<string> {
  auditLogId?: string;
  tenantId?: string;
  changeTime?: string;
  changeType: EntityChangeType;
  entityId?: string;
  entityTypeFullName?: string;
  propertyChanges: EntityPropertyChangeDto[];
}

export interface EntityChangeFilter {
  entityId?: string;
  entityTypeFullName?: string;
}

export interface EntityChangeWithUsernameDto {
  entityChange: EntityChangeDto;
  userName?: string;
}

export interface EntityPropertyChangeDto extends EntityDto<string> {
  tenantId?: string;
  entityChangeId?: string;
  newValue?: string;
  originalValue?: string;
  propertyName?: string;
  propertyTypeFullName?: string;
}

export interface GetAuditLogListDto extends PagedAndSortedResultRequestDto {
  startTime?: string;
  endTime?: string;
  url?: string;
  clientId?: string;
  userName?: string;
  applicationName?: string;
  clientIpAddress?: string;
  correlationId?: string;
  httpMethod?: string;
  httpStatusCode?: number;
  maxExecutionDuration?: number;
  minExecutionDuration?: number;
  hasException?: boolean;
}

export interface GetAverageExecutionDurationPerDayInput {
  startDate?: string;
  endDate?: string;
}

export interface GetAverageExecutionDurationPerDayOutput {
  data: Record<string, number>;
}

export interface GetEntityChangesDto extends PagedAndSortedResultRequestDto {
  auditLogId?: string;
  entityChangeType?: EntityChangeType;
  entityId?: string;
  entityTypeFullName?: string;
  startDate?: string;
  endDate?: string;
}

export interface GetErrorRateFilter {
  startDate?: string;
  endDate?: string;
}

export interface GetErrorRateOutput {
  data: Record<string, number>;
}
