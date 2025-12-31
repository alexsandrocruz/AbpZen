import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { eOpenIddictProComponents } from './enums';
import { ApplicationsComponent } from './components/applications/applications.component';
import { ScopesComponent } from './components/scopes/scopes.component';
import { openIddictProExtensionsResolver } from './resolvers/extensions.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'Applications', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [openIddictProExtensionsResolver],
    children: [
      {
        path: 'Applications',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'OpenIddictPro.Application',
          replaceableComponent: {
            key: eOpenIddictProComponents.Applications,
            defaultComponent: ApplicationsComponent,
          } as ReplaceableComponents.RouteData<ApplicationsComponent>,
        },
        title: 'AbpOpenIddict::Applications',
      },
      {
        path: 'Scopes',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'OpenIddictPro.Scopes',
          replaceableComponent: {
            key: eOpenIddictProComponents.Scopes,
            defaultComponent: ScopesComponent,
          } as ReplaceableComponents.RouteData<ScopesComponent>,
        },
        title: 'AbpOpenIddict::Scopes',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OpeniddictproRoutingModule {}
