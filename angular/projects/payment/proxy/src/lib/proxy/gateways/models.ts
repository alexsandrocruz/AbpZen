import type { EntityDto } from '@abp/ng.core';

export interface GatewayDto extends EntityDto {
  name?: string;
  displayName?: string;
}
