import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideTextTemplateManagementConfig } from './providers';

/**
 * @deprecated TextTemplateManagementConfigModule is deprecated use `provideTextTemplateManagementConfig` *function* instead.
 */
@NgModule()
export class TextTemplateManagementConfigModule {
  static forRoot(): ModuleWithProviders<TextTemplateManagementConfigModule> {
    return {
      ngModule: TextTemplateManagementConfigModule,
      providers: [provideTextTemplateManagementConfig()],
    };
  }
}
