import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { EditionDto } from '@volo/abp.ng.saas/proxy';

export const DEFAULT_EDITIONS_ENTITY_PROPS = EntityProp.createMany<EditionDto>([
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'Saas::EditionName',
    sortable: true,
  },
  {
    type: ePropType.Number,
    name: 'tenantCount',
    displayName: 'Saas::TenantCount',
  },
]);
