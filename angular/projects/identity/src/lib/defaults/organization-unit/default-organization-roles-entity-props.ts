import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { IdentityRoleDto } from '@volo/abp.ng.identity/proxy';

export const DEFAULT_ORGANIZATION_ROLES_ENTITY_PROPS = EntityProp.createMany<IdentityRoleDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::RoleName',
    sortable: true,
  },
]);
