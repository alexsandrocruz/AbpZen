import { Injector, inject } from '@angular/core';
import { AbpWindowService, RoutesService } from '@abp/ng.core';
import { Router } from '@angular/router';
import { eAccountRouteNames } from '../enums/route-names';

export function navigateToManageProfileFactory() {
  const routes = inject(RoutesService);
  const windowService = inject(AbpWindowService);
  return () => {
    const { path } = routes.find(item => item.name === eAccountRouteNames.ManageProfile);
    windowService.open(path, '_blank');
  };
}

export function navigateToMySecurityLogsFactory(injector: Injector) {
  return () => {
    const router = injector.get(Router);
    const routes = injector.get(RoutesService);
    const { path } = routes.find(item => item.name === eAccountRouteNames.MySecurityLogs);
    router.navigateByUrl(path);
  };
}

export function navigateToSessionsFactory() {
  const router = inject(Router);
  const routes = inject(RoutesService);

  return () => {
    const { path } = routes.find(item => item.name === eAccountRouteNames.Sessions);
    router.navigateByUrl(path);
  };
}
