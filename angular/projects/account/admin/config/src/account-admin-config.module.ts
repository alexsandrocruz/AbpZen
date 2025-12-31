import { CoreModule } from '@abp/ng.core';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AccountSettingsModule } from '@volo/abp.ng.account/admin';
import { provideAccountAdminConfig } from './providers';

@NgModule({
  imports: [CoreModule, AccountSettingsModule],
  exports: [AccountSettingsModule],
  declarations: [],
})
export class AccountAdminConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideAccountAdminConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<AccountAdminConfigModule> {
    return {
      ngModule: AccountAdminConfigModule,
      providers: [provideAccountAdminConfig()],
    };
  }
}
