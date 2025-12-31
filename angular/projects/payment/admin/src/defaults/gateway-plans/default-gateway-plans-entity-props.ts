import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { GatewayPlanDto } from '@volo/abp.ng.payment/proxy';

export const DEFAULT_GATEWAY_PLANS_ENTITY_PROPS = EntityProp.createMany<GatewayPlanDto>([
  {
    type: ePropType.String,
    name: 'gateway',
    displayName: 'Payment::DisplayName:Gateway',
    sortable: true,
  },
  {
    type: ePropType.String,
    name: 'externalId',
    displayName: 'Payment::DisplayName:ExternalId',
    sortable: true,
  },
]);
