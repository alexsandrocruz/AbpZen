import { eLayoutType, RoutesService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER } from '@angular/core';
import { eOpenIddictProPolicyNames } from '../enums/policy-names';
import { eOpenIddictProRouteNames } from '../enums/route-names';

export const OPENIDDICT_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        name: eOpenIddictProRouteNames.Default,
        path: '/openiddict',
        parentName: eThemeSharedRouteNames.Administration,
        layout: eLayoutType.application,
        iconClass: 'fa fa-id-badge',
        order: 3,
        requiredPolicy: eOpenIddictProPolicyNames.default,
      },
      {
        name: eOpenIddictProRouteNames.Applications,
        path: '/openiddict/Applications',
        parentName: eOpenIddictProRouteNames.Default,
        layout: eLayoutType.application,
        order: 1,
        requiredPolicy: eOpenIddictProPolicyNames.application,
      },
      {
        name: eOpenIddictProRouteNames.Scopes,
        path: '/openiddict/Scopes',
        parentName: eOpenIddictProRouteNames.Default,
        layout: eLayoutType.application,
        order: 2,
        requiredPolicy: eOpenIddictProPolicyNames.scope,
      },
    ]);
  };
}
