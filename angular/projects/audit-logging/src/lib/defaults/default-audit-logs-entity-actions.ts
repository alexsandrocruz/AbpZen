import { EntityAction } from '@abp/ng.components/extensible';
import { AuditLogDto } from '@volo/abp.ng.audit-logging/proxy';
import { AuditLogsComponent } from '../components/audit-logs/audit-logs.component';

export const DEFAULT_AUDIT_LOGS_ENTITY_ACTIONS = EntityAction.createMany<AuditLogDto>([
  {
    text: 'AbpAuditLogging::Detail',
    action: data => {
      const component = data.getInjected(AuditLogsComponent);
      component.openModal(data.record.id);
    },
    icon: 'fa fa-eye',
  },
]);
