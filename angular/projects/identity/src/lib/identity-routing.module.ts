import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClaimsComponent } from './components/claims/claims.component';
import { OrganizationUnitsComponent } from './components/organization-units/organization-units.component';
import { RolesComponent } from './components/roles/roles.component';
import { SecurityLogsComponent } from './components/security-logs/security-logs.component';
import { UsersComponent } from './components/users/users.component';
import { eIdentityComponents } from './enums/components';
import { identityExtensionsResolver } from './resolvers/extensions.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [identityExtensionsResolver],
    children: [
      {
        path: 'roles',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Roles',
          replaceableComponent: {
            key: eIdentityComponents.Roles,
            defaultComponent: RolesComponent,
          } as ReplaceableComponents.RouteData<RolesComponent>,
        },
        title: "AbpIdentity::Roles"
      },
      {
        path: 'users',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Users',
          replaceableComponent: {
            key: eIdentityComponents.Users,
            defaultComponent: UsersComponent,
          } as ReplaceableComponents.RouteData<UsersComponent>,
        },
        title: "AbpIdentity::Users"
      },
      {
        path: 'claim-types',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.ClaimTypes',
          replaceableComponent: {
            key: eIdentityComponents.Claims,
            defaultComponent: ClaimsComponent,
          } as ReplaceableComponents.RouteData<ClaimsComponent>,
        },
        title: "AbpIdentity::ClaimTypes"
      },
      {
        path: 'organization-units',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.OrganizationUnits',
          replaceableComponent: {
            key: eIdentityComponents.OrganizationUnits,
            defaultComponent: OrganizationUnitsComponent,
          } as ReplaceableComponents.RouteData<OrganizationUnitsComponent>,
        },
        title: "AbpIdentity::OrganizationUnits"
      },
      {
        path: 'security-logs',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.SecurityLogs',
          replaceableComponent: {
            key: eIdentityComponents.SecurityLogs,
            defaultComponent: SecurityLogsComponent,
          } as ReplaceableComponents.RouteData<SecurityLogsComponent>,
        },
        title: "AbpIdentity::SecurityLogs"
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityRoutingModule { }
