import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { GdprRequestDto } from '@volo/abp.ng.gdpr/proxy';
import { GdprActionColumnComponent } from '../components/gdpr-action-column/gdpr-action-column.component';
import { getStatus } from '../utils/get-status';

export const DEFAULT_PERSONAL_DATA_ENTITY_PROPS = EntityProp.createMany<GdprRequestDto>([
  {
    type: ePropType.String,
    name: 'action',
    displayName: 'AbpGdpr::Action',
    sortable: true,
    columnWidth: 180,
    component: GdprActionColumnComponent,
    valueResolver: data => getStatus(data.record),
  },
  {
    type: ePropType.DateTime,
    name: 'creationTime',
    displayName: 'AbpGdpr::CreationTime',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.DateTime,
    name: 'readyTime',
    displayName: 'AbpGdpr::ReadyTime',
    sortable: true,
    columnWidth: 200,
  },
]);
