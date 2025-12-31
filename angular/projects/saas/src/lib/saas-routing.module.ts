import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditionsComponent } from './components/editions/editions.component';
import { TenantsComponent } from './components/tenants/tenants.component';
import { eSaasComponents } from './enums/components';
import { saasExtensionsResolver } from './resolvers/extensions.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'tenants', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [saasExtensionsResolver],
    children: [
      {
        path: 'tenants',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Saas.Tenants',
          replaceableComponent: {
            key: eSaasComponents.Tenants,
            defaultComponent: TenantsComponent,
          } as ReplaceableComponents.RouteData<TenantsComponent>,
        },
        title: "Saas::Tenants"
      },
      {
        path: 'editions',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Saas.Editions',
          replaceableComponent: {
            key: eSaasComponents.Editions,
            defaultComponent: EditionsComponent,
          } as ReplaceableComponents.RouteData<EditionsComponent>,
        },
        title: "Saas::Editions"
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SaasRoutingModule { }
