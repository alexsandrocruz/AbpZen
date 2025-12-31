import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';
import { of } from 'rxjs';

export const DEFAULT_ORGANIZATION_MEMBERS_ENTITY_PROPS = EntityProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    sortable: true,
    columnWidth: 180,
  },
  {
    type: ePropType.String,
    name: 'email',
    displayName: 'AbpIdentity::EmailAddress',
    sortable: true,
    columnWidth: 200,
    valueResolver: data => {
      const { email, emailConfirmed } = data.record;

      return of(
        (email || '') + (emailConfirmed ? `<i class="fa fa-check text-success ms-1"></i>` : ''),
      );
    },
  },
]);
