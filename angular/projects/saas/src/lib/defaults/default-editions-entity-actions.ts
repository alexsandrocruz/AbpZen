import { EntityAction } from '@abp/ng.components/extensible';
import { SHOW_ENTITY_HISTORY } from '@volo/abp.commercial.ng.ui';
import { EditionDto } from '@volo/abp.ng.saas/proxy';
import { EditionsComponent } from '../components/editions/editions.component';

export const DEFAULT_EDITIONS_ENTITY_ACTIONS = EntityAction.createMany<EditionDto>([
  {
    text: 'Saas::Edit',
    action: data => {
      const component = data.getInjected(EditionsComponent);
      component.onEditEdition(data.record.id);
    },
    permission: 'Saas.Editions.Update',
  },
  {
    text: 'Saas::Features',
    action: data => {
      const component = data.getInjected(EditionsComponent);
      component.openFeaturesModal(data.record.id, data.record.displayName);
    },
    permission: 'Saas.Editions.ManageFeatures',
  },
  {
    text: 'Saas::ChangeHistory',
    action: data => {
      const showHistory = data.getInjected(SHOW_ENTITY_HISTORY);
      showHistory(data.record.id, 'Volo.Saas.Editions.Edition');
    },
    permission: 'AuditLogging.ViewChangeHistory:Volo.Saas.Edition',
    visible: data => Boolean(data.getInjected(SHOW_ENTITY_HISTORY, null)),
  },
  {
    text: 'Saas::Delete',
    action: data => {
      const component = data.getInjected(EditionsComponent);
      component.delete(data.record);
    },
    permission: 'Saas.Editions.Delete',
  },
  {
    text: 'Saas::MoveAllTenants',
    action: data => {
      const component = data.getInjected(EditionsComponent);
      component.moveAllTenants(data.record);
    },
    permission: 'Saas.Tenants.Update',
  },
]);
