import { EntityAction } from '@abp/ng.components/extensible';
import { SHOW_ENTITY_HISTORY } from '@volo/abp.commercial.ng.ui';
import { IdentityRoleDto } from '@volo/abp.ng.identity/proxy';
import { RolesComponent } from '../../components/roles/roles.component';

export const DEFAULT_ROLES_ENTITY_ACTIONS = EntityAction.createMany<IdentityRoleDto>([
  {
    text: 'AbpUi::Edit',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.onEdit(data.record.id);
    },
    permission: 'AbpIdentity.Roles.Update',
  },
  {
    text: 'AbpIdentity::Claims',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.onManageClaims(data.record);
    },
    permission: 'AbpIdentity.Roles.Update',
  },
  {
    text: 'AbpIdentity::Permissions',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.openPermissionsModal(data.record.name);
    },
    permission: 'AbpIdentity.Roles.ManagePermissions',
  },
  {
    text: 'AbpIdentity::ChangeHistory',
    action: data => {
      const showHistory = data.getInjected(SHOW_ENTITY_HISTORY);
      showHistory(data.record.id, 'Volo.Abp.Identity.IdentityRole');
    },
    permission: 'AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityRole',
    visible: data => Boolean(data.getInjected(SHOW_ENTITY_HISTORY, null)),
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.delete(data.record);
    },
    permission: 'AbpIdentity.Roles.Delete',
    visible: data => !data.record.isStatic,
  },
  {
    text: 'AbpIdentity::MoveAllUsers',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.moveAllUsers(data.record);
    },
    permission: 'AbpIdentity.Roles.Update',
  },
]);
