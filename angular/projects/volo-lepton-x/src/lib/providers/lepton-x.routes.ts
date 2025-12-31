import { EnvironmentProviders } from '@angular/core';
import { provideRouter } from '@angular/router';
import { ThemeLeptonXModuleOptions } from '../lepton-x.module';

export function leptonXRoutes(
  options: ThemeLeptonXModuleOptions
): EnvironmentProviders {
  const { httpError } = options || {};

  return provideRouter([
    {
      pathMatch: 'full',
      path: httpError?.errorPath || 'error',
      loadComponent: () =>
        import('./../components/http-error/http-error.component').then(
          (c) => c.HttpErrorComponent
        ),
    },
  ]);
}
