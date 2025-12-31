import { EnvironmentProviders, Provider } from '@angular/core';
import { ThemeLeptonXModuleOptions } from '../lepton-x.module';
import { CreateErrorComponentService } from '@abp/ng.theme.shared';
import { AbpCreateErrorComponentService } from '../services';
import { HTTP_ERROR_PATH } from '../tokens';
import { leptonXRoutes } from './lepton-x.routes';

export function httpErrorProvider(
  options: ThemeLeptonXModuleOptions
): (Provider | EnvironmentProviders)[] {
  const { httpError } = options || {};

  return [
    {
      provide: HTTP_ERROR_PATH,
      useValue: httpError?.errorPath || 'error',
    },
    {
      provide: CreateErrorComponentService,
      useClass: AbpCreateErrorComponentService,
    },
    leptonXRoutes(options),
  ];
}
