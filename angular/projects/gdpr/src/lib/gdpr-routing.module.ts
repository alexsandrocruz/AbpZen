import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GdprComponent } from './components/gdpr.component';
import { eGdprComponents } from './enums/components';
import { gdprExtensionsResolver } from './resolvers';

const routes: Routes = [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [gdprExtensionsResolver],
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          replaceableComponent: {
            defaultComponent: GdprComponent,
            key: eGdprComponents.PersonalData,
          } as ReplaceableComponents.RouteData<GdprComponent>,
        },
        title: 'AbpGdpr::PersonalData',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GdprRoutingModule {}
