import { ToolbarAction } from '@abp/ng.components/extensible';
import { AuditLogDto } from '@volo/abp.ng.audit-logging/proxy';

export const DEFAULT_AUDIT_LOGS_TOOLBAR_ACTIONS = ToolbarAction.createMany<AuditLogDto[]>([]);
