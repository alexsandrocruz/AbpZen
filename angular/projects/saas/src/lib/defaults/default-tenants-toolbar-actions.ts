import { ToolbarAction } from '@abp/ng.components/extensible';
import { SaasTenantDto } from '@volo/abp.ng.saas/proxy';
import { TenantsComponent } from '../components/tenants/tenants.component';

export const DEFAULT_TENANTS_TOOLBAR_ACTIONS = ToolbarAction.createMany<SaasTenantDto[]>([
  {
    text: 'Saas::NewTenant',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.onAddTenant();
    },
    permission: 'Saas.Tenants.Create',
    icon: 'fa fa-plus',
  },
]);
