import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideOpeniddictproConfig } from './providers';

/**
 * @deprecated OpeniddictproConfigModule is deprecated use `provideOpeniddictproConfig` *function* instead.
 */
@NgModule()
export class OpeniddictproConfigModule {
  static forRoot(): ModuleWithProviders<OpeniddictproConfigModule> {
    return {
      ngModule: OpeniddictproConfigModule,
      providers: [provideOpeniddictproConfig()],
    };
  }
}
