import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbDropdownModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  IdentitySettingsComponent,
  MyLinkUsersModalComponent,
  IdentitySettingComponent,
  IdentityExternalLoginSettingsComponent,
  IdentityLdapSettingsComponent,
  AuthorityDelegationModule,
  IdentitySessionsSettingComponent
} from './components';
import { provideIdentityConfig } from './providers';

const declarationsWithExports = [
  IdentitySettingsComponent,
  IdentitySettingComponent,
  IdentityExternalLoginSettingsComponent,
  IdentityLdapSettingsComponent,
  MyLinkUsersModalComponent,
];

@NgModule({
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgbNavModule,
    NgxValidateCoreModule,
    AuthorityDelegationModule,
    IdentitySessionsSettingComponent,
  ],
  declarations: [...declarationsWithExports],
  exports: [...declarationsWithExports],
})
export class IdentityConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideIdentityConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<IdentityConfigModule> {
    return {
      ngModule: IdentityConfigModule,
      providers: [provideIdentityConfig()],
    };
  }
}
