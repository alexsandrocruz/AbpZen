import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { Applications } from '@volo/abp.ng.openiddictpro/proxy';

export const DEFAULT_APPLICATIONS_ENTITY_PROPS =
  EntityProp.createMany<Applications.Dtos.ApplicationDto>([
    {
      type: ePropType.String,
      name: 'applicationType',
      displayName: 'AbpOpenIddict::ApplicationType',
      columnWidth: 100,
    },
    {
      type: ePropType.String,
      name: 'clientId',
      displayName: 'AbpOpenIddict::ClientId',
      columnWidth: 200,
    },
    {
      type: ePropType.String,
      name: 'displayName',
      displayName: 'AbpOpenIddict::DisplayName',
      columnWidth: 100,
    },

    {
      type: ePropType.String,
      name: 'clientType',
      displayName: 'AbpOpenIddict::ClientType',
      columnWidth: 200,
    },
  ]);
