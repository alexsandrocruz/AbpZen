import { ToolbarAction } from '@abp/ng.components/extensible';
import { IdentityRoleDto } from '@volo/abp.ng.identity/proxy';
import { RolesComponent } from '../../components/roles/roles.component';

export const DEFAULT_ROLES_TOOLBAR_ACTIONS = ToolbarAction.createMany<IdentityRoleDto[]>([
  {
    text: 'AbpIdentity::NewRole',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.onAdd();
    },
    permission: 'AbpIdentity.Roles.Create',
    icon: 'fa fa-plus',
  },
]);
