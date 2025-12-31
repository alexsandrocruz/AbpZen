import { ToolbarAction } from '@abp/ng.components/extensible';
import { EditionDto } from '@volo/abp.ng.saas/proxy';
import { EditionsComponent } from '../components/editions/editions.component';

export const DEFAULT_EDITIONS_TOOLBAR_ACTIONS = ToolbarAction.createMany<EditionDto[]>([
  {
    text: 'Saas::NewEdition',
    action: data => {
      const component = data.getInjected(EditionsComponent);
      component.onAddEdition();
    },
    permission: 'Saas.Editions.Create',
    icon: 'fa fa-plus',
  },
]);
