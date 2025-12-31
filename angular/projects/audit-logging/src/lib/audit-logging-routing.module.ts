import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditLogsComponent } from './components/audit-logs/audit-logs.component';
import { eAuditLoggingComponents } from './enums/components';
import { auditLoggingGuard } from './guards/audit-logging.guard';
import { auditLoggingExtensionsResolver } from './resolvers/extensions.resolver';

const routes: Routes = [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard, auditLoggingGuard],
    resolve: [auditLoggingExtensionsResolver],
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AuditLogging.AuditLogs',
          replaceableComponent: {
            key: eAuditLoggingComponents.AuditLogs,
            defaultComponent: AuditLogsComponent,
          } as ReplaceableComponents.RouteData<AuditLogsComponent>,
        },
        title: 'AbpAuditLogging::Menu:AuditLogging',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuditLoggingRoutingModule {}
