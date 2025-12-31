import { ConfigStateService, featuresFactory, noop, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER, inject, InjectionToken } from '@angular/core';
import { ModuleVisibility, setModuleVisibilityFactory } from '@volo/abp.commercial.ng.ui/config';
import { Observable } from 'rxjs';
import { eTextTemplateManagementRouteNames } from '../enums/route-names';

export const TEXT_TEMPLATE_MANAGEMENT_FEATURES = new InjectionToken<Observable<ModuleVisibility>>(
  'TEXT_TEMPLATE_MANAGEMENT_FEATURES',
  {
    providedIn: 'root',
    factory: () => {
      const configState = inject(ConfigStateService);
      const featureKey = 'TextManagement.Enable';
      const mapFn = features => ({
        enable: features[featureKey].toLowerCase() !== 'false',
      });

      return featuresFactory(configState, [featureKey], mapFn);
    },
  },
);

export const SET_TEXT_TEMPLATE_MANAGEMENT_ROUTE_VISIBILITY = new InjectionToken(
  'SET_TEXT_TEMPLATE_MANAGEMENT_ROUTE_VISIBILITY',
  {
    providedIn: 'root',
    factory: () => {
      const routes = inject(RoutesService);
      const stream = inject(TEXT_TEMPLATE_MANAGEMENT_FEATURES);

      setModuleVisibilityFactory(
        stream,
        routes,
        eTextTemplateManagementRouteNames.TextTemplates,
      ).subscribe();
    },
  },
);

export const TEXT_TEMPLATE_MANAGEMENT_FEATURES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: noop,
    deps: [SET_TEXT_TEMPLATE_MANAGEMENT_ROUTE_VISIBILITY],
    multi: true,
  },
];
