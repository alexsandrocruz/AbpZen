import { EntityAction } from '@abp/ng.components/extensible';
import { SHOW_ENTITY_HISTORY } from '@volo/abp.commercial.ng.ui';
import { Scopes } from '@volo/abp.ng.openiddictpro/proxy';
import { ScopesComponent } from '../components/scopes/scopes.component';

export const DEFAULT_SCOPE_ENTITY_ACTIONS = EntityAction.createMany<Scopes.Dtos.ScopeDto>([
  {
    text: 'AbpOpenIddict::Edit',
    action: data => {
      const component = data.getInjected(ScopesComponent);
      component.onEdit(data.record.id);
    },
    permission: 'OpenIddictPro.Scope.Update',
  },

  {
    text: 'AbpOpenIddict::ChangeHistory',
    action: data => {
      const showHistory = data.getInjected(SHOW_ENTITY_HISTORY);
      showHistory(data.record.id, 'Volo.Abp.OpenIddict.Scopes.OpenIddictScope');
    },
    permission: 'AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Scopes.Scope',
    visible: data => Boolean(data.getInjected(SHOW_ENTITY_HISTORY, null)),
  },
  {
    text: 'AbpOpenIddict::Delete',
    action: data => {
      const component = data.getInjected(ScopesComponent);
      component.onDelete(data.record.id, data.record.name);
    },
    permission: 'OpenIddictPro.Scope.Delete',
  },
]);
