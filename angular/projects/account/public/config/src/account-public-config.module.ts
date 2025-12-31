import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideAccountPublicConfig } from './providers';

/**
 * @deprecated AccountPublicConfigModule is deprecated use `provideAccountPublicConfig` *function* instead.
 */
@NgModule()
export class AccountPublicConfigModule {
  static forRoot(): ModuleWithProviders<AccountPublicConfigModule> {
    return {
      ngModule: AccountPublicConfigModule,
      providers: [provideAccountPublicConfig()],
    };
  }
}
