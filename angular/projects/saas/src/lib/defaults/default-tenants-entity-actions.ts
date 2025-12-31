import { ConfigStateService, EnvironmentService } from '@abp/ng.core';
import { EntityAction } from '@abp/ng.components/extensible';
import { SHOW_ENTITY_HISTORY } from '@volo/abp.commercial.ng.ui';
import { SaasTenantDto, TenantActivationState } from '@volo/abp.ng.saas/proxy';
import { TenantsComponent } from '../components/tenants/tenants.component';
import { ImpersonateTenantModalService } from '../services/impersonate-tenant-modal.service';

export const DEFAULT_TENANTS_ENTITY_ACTIONS = EntityAction.createMany<SaasTenantDto>([
  {
    text: 'Saas::Edit',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.onEditTenant(data.record.id);
    },
    permission: 'Saas.Tenants.Update',
  },
  {
    text: 'Saas::ConnectionStrings',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.selected.set(data.record);
      component.databaseConnectionStringsForm.reset();
      component.isConnectionStringModalVisible.set(true);
    },
    permission: 'Saas.Tenants.ManageConnectionStrings',
  },
  {
    text: 'Saas::ApplyDatabaseMigrations',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.applyDatabaseMigrations(data.record.id);
    },
    permission: 'Saas.Tenants.ManageConnectionStrings',
    visible: data => data.record.hasDefaultConnectionString,
  },
  {
    text: 'Saas::Features',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.openFeaturesModal(data.record.id, data.record.name);
    },
    permission: 'Saas.Tenants.ManageFeatures',
  },
  {
    text: 'Saas::ChangeHistory',
    action: data => {
      const showHistory = data.getInjected(SHOW_ENTITY_HISTORY);
      showHistory(data.record.id, 'Volo.Saas.Tenants.Tenant');
    },
    permission: 'AuditLogging.ViewChangeHistory:Volo.Saas.Tenant',
    visible: data => Boolean(data.getInjected(SHOW_ENTITY_HISTORY, null)),
  },
  {
    text: 'Saas::LoginWithThisTenant',
    action: data => {
      const impersonateTenantModalService = data.getInjected(ImpersonateTenantModalService);
      const component = data.getInjected(TenantsComponent);
      component.selected.set(data.record);
      impersonateTenantModalService.show(data.record.id);
    },
    permission: 'Saas.Tenants.Impersonation',
    visible: data => {
      const configState = data.getInjected(ConfigStateService);
      const environmentService = data.getInjected(EnvironmentService);

      const impersonatorUserId = configState.getDeep('currentUser.impersonatorUserId');
      const { activationState, activationEndDate } = data.record || {};

      const impersonation = environmentService.getImpersonation();
      if (!impersonation || !impersonation.tenantImpersonation) {
        return false;
      }

      const isActive = activationState == TenantActivationState.Active;
      const isActiveWithTime = activationState == TenantActivationState.ActiveWithLimitedTime;
      const isGreaterThanNow = new Date(activationEndDate) > new Date();
      const isImpersonatorExits = impersonatorUserId === null;

      return (isActive || (isActiveWithTime && isGreaterThanNow)) && isImpersonatorExits;
    },
  },
  {
    text: 'Saas::SetPassword',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.openSetTenantPasswordModal(data.record.id, data.record.name);
    },
    permission: 'Saas.Tenants.SetPassword',
  },
  {
    text: 'Saas::Delete',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.delete(data.record.id, data.record.name);
    },
    permission: 'Saas.Tenants.Delete',
  },
]);
