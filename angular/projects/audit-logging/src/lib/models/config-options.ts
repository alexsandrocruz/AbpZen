import {
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { AuditLogDto, EntityChangeDto } from '@volo/abp.ng.audit-logging/proxy';
import { eAuditLoggingComponents } from '../enums/components';

export type AuditLoggingEntityActionContributors = Partial<{
  [eAuditLoggingComponents.AuditLogs]: EntityActionContributorCallback<AuditLogDto>[];
  [eAuditLoggingComponents.EntityChanges]: EntityActionContributorCallback<EntityChangeDto>[];
}>;

export type AuditLoggingToolbarActionContributors = Partial<{
  [eAuditLoggingComponents.AuditLogs]: ToolbarActionContributorCallback<AuditLogDto[]>[];
}>;

export type AuditLoggingEntityPropContributors = Partial<{
  [eAuditLoggingComponents.AuditLogs]: EntityPropContributorCallback<AuditLogDto>[];
}>;

export interface AuditLoggingConfigOptions {
  entityActionContributors?: AuditLoggingEntityActionContributors;
  toolbarActionContributors?: AuditLoggingToolbarActionContributors;
  entityPropContributors?: AuditLoggingEntityPropContributors;
}
