import type { ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GatewayPlanCreateInput extends ExtensibleObject {
  gateway: string;
  externalId: string;
}

export interface GatewayPlanGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface GatewayPlanUpdateInput extends ExtensibleObject {
  externalId: string;
}

export interface PlanCreateInput extends ExtensibleObject {
  name: string;
}

export interface PlanGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface PlanUpdateInput extends PlanCreateInput {
  concurrencyStamp?: string;
}
