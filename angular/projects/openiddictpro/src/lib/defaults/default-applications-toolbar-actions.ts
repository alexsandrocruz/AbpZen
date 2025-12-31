import { ToolbarAction } from '@abp/ng.components/extensible';
import { Applications } from '@volo/abp.ng.openiddictpro/proxy';
import { ApplicationsComponent } from '../components/applications/applications.component';

export const DEFAULT_APPLICATIONS_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  Applications.Dtos.ApplicationDto[]
>([
  {
    text: 'AbpOpenIddict::NewApplication',
    action: data => {
      const component = data.getInjected(ApplicationsComponent);
      component.onAdd();
    },
    permission: 'OpenIddictPro.Application.Create',
    icon: 'fa fa-plus',
  },
]);
