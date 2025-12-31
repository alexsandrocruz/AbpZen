import { ConfigStateService, featuresFactory, noop, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER, inject, InjectionToken } from '@angular/core';
import { ModuleVisibility, setModuleVisibilityFactory } from '@volo/abp.commercial.ng.ui/config';
import { Observable } from 'rxjs';
import { eLanguageManagementRouteNames } from '../enums/route-names';

export const LANGUAGE_MANAGEMENT_FEATURES = new InjectionToken<Observable<ModuleVisibility>>(
  'LANGUAGE_MANAGEMENT_FEATURES',
  {
    providedIn: 'root',
    factory: () => {
      const configState = inject(ConfigStateService);
      const featureKey = 'LanguageManagement.Enable';
      const mapFn = features => ({
        enable: features[featureKey].toLowerCase() !== 'false',
      });

      return featuresFactory(configState, [featureKey], mapFn);
    },
  },
);

export const SET_LANGUAGE_MANAGEMENT_ROUTE_VISIBILITY = new InjectionToken(
  'SET_LANGUAGE_MANAGEMENT_ROUTE_VISIBILITY',
  {
    providedIn: 'root',
    factory: () => {
      const routes = inject(RoutesService);
      const stream = inject(LANGUAGE_MANAGEMENT_FEATURES);

      setModuleVisibilityFactory(
        stream,
        routes,
        eLanguageManagementRouteNames.LanguageManagement,
      ).subscribe();
    },
  },
);

export const LANGUAGE_MANAGEMENT_FEATURES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: noop,
    deps: [SET_LANGUAGE_MANAGEMENT_ROUTE_VISIBILITY],
    multi: true,
  },
];
