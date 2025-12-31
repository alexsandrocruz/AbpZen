import {
  InjectionToken,
  inject,
  APP_INITIALIZER,
  Injector,
} from '@angular/core';
import {
  ConfigStateService,
  noop,
  RoutesService,
  featuresFactory,
} from '@abp/ng.core';
import { filter, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { eFileManagementRouteNames } from '../enums/route-names';
import {
  ModuleVisibility,
  setModuleVisibilityFactory,
} from '@volo/abp.commercial.ng.ui/config';

export const FILE_MANAGEMENT_FEATURES = new InjectionToken<
  Observable<ModuleVisibility>
>('FILE_MANAGEMENT_FEATURES', {
  providedIn: 'root',
  factory: () => {
    const configState = inject(ConfigStateService);
    const featureKey = 'FileManagement.Enable';
    const mapFn = (features) => ({
      enable: features[featureKey].toLowerCase() !== 'false',
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
