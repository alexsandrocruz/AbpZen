import { Provider, APP_INITIALIZER } from '@angular/core';
import { RoutesService, eLayoutType } from '@abp/ng.core';
import { eGdprComponents } from '@volo/abp.ng.gdpr';

export const GDPR_ROUTE_PROVIDERS: Provider[] = [
  {
    provide: APP_INITIALIZER,
    multi: true,
    deps: [RoutesService],
    useFactory: configureRoutes,
  },
];

export function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/gdpr',
        name: eGdprComponents.PersonalData,
        invisible: true,
        layout: eLayoutType.application,
        breadcrumbText: 'AbpGdpr::Menu:PersonalData',
        iconClass: 'fa fa-lock',
      },
    ]);
  };
}
