import { PagedResultDto } from '@abp/ng.core';
import { AuditLogDto } from '@volo/abp.ng.audit-logging/proxy';

export namespace AuditLogging {
  export interface State {
    result: PagedResultDto<AuditLogDto>;
    averageExecutionStatistics: Record<string, number>;
    errorRateStatistics: Record<string, number>;
  }
}
