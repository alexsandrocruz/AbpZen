import {
  ConfigStateService,
  featuresFactory,
  noop,
  RoutesService,
} from '@abp/ng.core';
import { APP_INITIALIZER, inject, InjectionToken } from '@angular/core';
import {
  ModuleVisibility,
  setModuleVisibilityFactory,
} from '@volo/abp.commercial.ng.ui/config';
import { Observable } from 'rxjs';
import { eFileManagementRouteNames } from '../enums/route-names';

export const FILE_MANAGEMENT_FEATURES = new InjectionToken<
  Observable<ModuleVisibility>
>('FILE_MANAGEMENT_FEATURES', {
  providedIn: 'root',
  factory: () => {
    const configState = inject(ConfigStateService);
    const featureKey = 'FileManagement.Enable';
    const mapFn = (features) => ({
      enable: features[featureKey]?.toLowerCase() === 'true'
    });

    return featuresFactory(configState, [featureKey], mapFn);
  },
});

export const SET_FILE_MANAGEMENT_ROUTE_VISIBILITY = new InjectionToken(
  'SET_FILE_MANAGEMENT_ROUTE_VISIBILITY',
  {
    providedIn: 'root',
    factory: () => {
      const routes = inject(RoutesService);
      const stream = inject(FILE_MANAGEMENT_FEATURES);

      setModuleVisibilityFactory(
        stream,
        routes,
        eFileManagementRouteNames.FileManagement
      ).subscribe();
    },
  }
);

export const FILE_MANAGEMENT_FEATURES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: noop,
    deps: [SET_FILE_MANAGEMENT_ROUTE_VISIBILITY],
    multi: true,
  },
];
