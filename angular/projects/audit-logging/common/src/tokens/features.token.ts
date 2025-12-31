import { ConfigStateService, featuresFactory, noop, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER, inject, InjectionToken } from '@angular/core';
import { ModuleVisibility, setModuleVisibilityFactory } from '@volo/abp.commercial.ng.ui/config';
import { Observable } from 'rxjs';
import { eAuditLoggingRouteNames } from '../enums/route-names';

export const AUDIT_LOGGING_FEATURES = new InjectionToken<Observable<ModuleVisibility>>(
  'AUDIT_LOGGING_FEATURES',
  {
    providedIn: 'root',
    factory: () => {
      const configState = inject(ConfigStateService);
      const featureKey = 'AuditLogging.Enable';
      const mapFn = features => ({
        enable: features[featureKey].toLowerCase() !== 'false',
      });

      return featuresFactory(configState, [featureKey], mapFn);
    },
  },
);

export const SET_AUDIT_LOGGING_ROUTE_VISIBILITY = new InjectionToken(
  'SET_AUDIT_LOGGING_ROUTE_VISIBILITY',
  {
    providedIn: 'root',
    factory: () => {
      const routes = inject(RoutesService);
      const stream = inject(AUDIT_LOGGING_FEATURES);

      setModuleVisibilityFactory(stream, routes, eAuditLoggingRouteNames.AuditLogging).subscribe();
    },
  },
);

export const AUDIT_LOGGING_FEATURES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: noop,
    deps: [SET_AUDIT_LOGGING_ROUTE_VISIBILITY],
    multi: true,
  },
];
