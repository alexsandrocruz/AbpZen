import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { Scopes } from '@volo/abp.ng.openiddictpro/proxy';

export const DEFAULT_SCOPES_ENTITY_PROPS = EntityProp.createMany<Scopes.Dtos.ScopeDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpOpenIddict::Name',
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'AbpOpenIddict::DisplayName',
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'description',
    displayName: 'AbpOpenIddict::Description',
    columnWidth: 200,
  },
]);
