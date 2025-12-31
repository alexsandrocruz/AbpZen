import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideLanguageManagementConfig } from './providers';

/**
 * @deprecated LanguageManagementConfigModule is deprecated use `provideLanguageManagementConfig` *function* instead.
 */
@NgModule()
export class LanguageManagementConfigModule {
  static forRoot(): ModuleWithProviders<LanguageManagementConfigModule> {
    return {
      ngModule: LanguageManagementConfigModule,
      providers: [provideLanguageManagementConfig()],
    };
  }
}
