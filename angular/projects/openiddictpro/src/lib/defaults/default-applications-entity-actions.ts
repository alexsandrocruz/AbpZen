import { EntityAction } from '@abp/ng.components/extensible';
import { SHOW_ENTITY_HISTORY } from '@volo/abp.commercial.ng.ui';
import { Applications } from '@volo/abp.ng.openiddictpro/proxy';
import { ApplicationsComponent } from '../components/applications/applications.component';

export const DEFAULT_APPLICATIONS_ENTITY_ACTIONS =
  EntityAction.createMany<Applications.Dtos.ApplicationDto>([
    {
      text: 'AbpOpenIddict::Edit',
      action: data => {
        const component = data.getInjected(ApplicationsComponent);
        component.edit(data.record.id);
      },
      permission: 'OpenIddictPro.Application.Update',
    },
    {
      text: 'AbpOpenIddict::TokenLifetime',
      action: data => {
        const component = data.getInjected(ApplicationsComponent);
        component.openTokenLifetimeModal(data.record);
      },
      permission: 'OpenIddictPro.Application.Update',
    },
    {
      text: 'AbpOpenIddict::ChangeHistory',
      action: data => {
        const showHistory = data.getInjected(SHOW_ENTITY_HISTORY);
        showHistory(data.record.id, 'Volo.Abp.OpenIddict.Applications.OpenIddictApplication');
      },
      permission: 'AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Applications.Application',
    },
    {
      text: 'AbpOpenIddict::Permissions',
      action: data => {
        const component = data.getInjected(ApplicationsComponent);
        component.openPermissionsModal(data.record.clientId);
      },
      permission: 'OpenIddictPro.Application.ManagePermissions',
    },
    {
      text: 'LanguageManagement::Delete',
      action: data => {
        const component = data.getInjected(ApplicationsComponent);
        component.delete(data.record.id, data.record.clientId);
      },
      permission: 'OpenIddictPro.Application.Delete',
    },
  ]);
