import { ToolbarAction, ToolbarComponent } from '@abp/ng.components/extensible';
import { GdprRequestDto } from '@volo/abp.ng.gdpr/proxy';
import { GdprComponent } from '../components/gdpr.component';
import { RequestPersonalDataToolbarActionComponent } from '../components/request-personal-data-toolbar-action/request-personal-data-toolbar-action.component';

export const DEFAULT_PERSONAL_DATA_TOOLBAR_COMPONENTS = ToolbarComponent.createMany<
  GdprRequestDto[]
>([
  {
    component: RequestPersonalDataToolbarActionComponent,
  },
]);

export const DEFAULT_PERSONAL_DATA_TOOLBAR_ACTIONS = ToolbarAction.createMany<GdprRequestDto[]>([
  {
    text: 'AbpGdpr::DeletePersonalData',
    action: data => {
      const component = data.getInjected(GdprComponent);
      component.deleteData();
    },
    btnClass: 'btn btn-sm btn-danger',
  },
]);

export const DEFAULT_GDPR_TOOLBAR = [
  ...DEFAULT_PERSONAL_DATA_TOOLBAR_COMPONENTS,
  ...DEFAULT_PERSONAL_DATA_TOOLBAR_ACTIONS,
];
