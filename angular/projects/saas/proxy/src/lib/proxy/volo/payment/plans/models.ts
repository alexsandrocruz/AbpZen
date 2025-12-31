import type { ExtensibleEntityDto } from '@abp/ng.core';

export interface PlanDto extends ExtensibleEntityDto<string> {
  name?: string;
  concurrencyStamp?: string;
}
