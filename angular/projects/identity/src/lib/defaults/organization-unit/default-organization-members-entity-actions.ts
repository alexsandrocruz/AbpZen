/* tslint:disable:max-line-length */
import { EntityAction } from '@abp/ng.components/extensible';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';
import { OrganizationMembersComponent } from '../../components/organization-units/organization-members/organization-members.component';

export const DEFAULT_ORGANIZATION_MEMBERS_ENTITY_ACTIONS = EntityAction.createMany<IdentityUserDto>(
  [
    {
      text: 'AbpUi::Delete',
      permission: 'AbpIdentity.OrganizationUnits.ManageMembers',
      btnClass: 'btn btn-danger text-center',
      icon: 'fa fa-trash',
      showOnlyIcon: true,
      tooltip: { text: 'AbpUi::Delete' },
      action: data => {
        const component = data.getInjected(OrganizationMembersComponent);
        component.delete(data.record.id, data.record.userName);
      },
    },
  ],
);
