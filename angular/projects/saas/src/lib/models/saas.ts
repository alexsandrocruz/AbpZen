import { PagedResultDto } from '@abp/ng.core';
import {EditionDto,  SaasTenantDto} from '@volo/abp.ng.saas/proxy';

export namespace Saas {
  export interface State {
    tenants: PagedResultDto<SaasTenantDto>;
    editions: PagedResultDto<EditionDto>;
    usageStatistics: Record<string, number>;
    latestTenants: SaasTenantDto[];
  }
}
