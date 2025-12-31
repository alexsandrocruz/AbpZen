import type { ExtensibleEntityDto } from '@abp/ng.core';

export interface GatewayPlanDto extends ExtensibleEntityDto {
  planId?: string;
  gateway?: string;
  externalId?: string;
}

export interface PlanDto extends ExtensibleEntityDto<string> {
  name?: string;
  concurrencyStamp?: string;
}
