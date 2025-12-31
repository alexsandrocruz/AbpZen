import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { PlanDto } from '@volo/abp.ng.payment/proxy';

export const DEFAULT_PLANS_ENTITY_PROPS = EntityProp.createMany<PlanDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'Payment::DisplayName:Name',
    sortable: true,
  },
]);
