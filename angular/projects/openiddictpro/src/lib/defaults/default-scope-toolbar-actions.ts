import { ToolbarAction } from '@abp/ng.components/extensible';
import { Applications } from '@volo/abp.ng.openiddictpro/proxy';
import { ScopesComponent } from '../components/scopes/scopes.component';

export const DEFAULT_SCOPES_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  Applications.Dtos.ApplicationDto[]
>([
  {
    text: 'AbpOpenIddict::NewScope',
    action: data => {
      const component = data.getInjected(ScopesComponent);
      component.onAdd();
    },
    permission: 'OpenIddictPro.Scope.Create',
    icon: 'fa fa-plus',
  },
]);
