import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import {
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbPopoverModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { PageModule } from '@abp/ng.components/page';
import {
  CoreModule,
  LazyModuleFactory,
  ShowPasswordDirective,
  TrackCapsLockDirective,
} from '@abp/ng.core';
import { PasswordComponent, ThemeSharedModule } from '@abp/ng.theme.shared';

import { CommercialUiModule, DatetimePickerComponent } from '@volo/abp.commercial.ng.ui';
import { TimeZoneSettingComponent } from '@volo/abp.ng.account/admin';

import { AccountConfigOptions } from './models/config-options';

import { RecoveryCodeService } from './services/recovery-code.service';
import { SecurityCodeService } from './services/security-code.service';

import { AccountPublicRoutingModule } from './account-public-routing.module';

import { ACCOUNT_CONFIG_OPTIONS } from './tokens/config-options.token';
import {
  ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
  ACCOUNT_ENTITY_ACTION_CONTRIBUTORS,
  ACCOUNT_ENTITY_PROP_CONTRIBUTORS,
  ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';

import { ManageProfileResolver } from './resolvers/manage-profile.resolver';

import { accountOptionsFactory } from './utils/factory-utils';

import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LinkLoggedComponent } from './components/link-logged/link-logged.component';
import { LoginComponent } from './components/login/login.component';
import { MySecurityLogsComponent } from './components/my-security-logs/my-security-logs.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { ProfilePictureComponent } from './components/profile-picture/profile-picture.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { SendSecurityCodeComponent } from './components/send-securiy-code/send-security-code.component';
import { TwoFactorTabComponent } from './components/two-factor-tab/two-factor-tab.component';
import { PasswordComplexityIndicatorComponent } from './components/password-complexity-indicator/password-complexity-indicator.component';

import { PersonalSettingsVerifyButtonComponent } from './components/personal-settings/personal-settings-verify-button/personal-settings-verify-button.component';
import { PersonalSettingsComponent } from './components/personal-settings/personal-settings.component';
import { PersonalSettingsHalfRowComponent } from './components/personal-settings/personal-settings-half-row/personal-settings-half-row.component';
import { PersonalSettingsEmailComponent } from './components/personal-settings/personal-settings-email/personal-settings-email.component';
import { PersonalSettingsPhoneNumberComponent } from './components/personal-settings/personal-settings-phone-number/personal-settings-phone-number.component';
import { PersonalSettingsTimerZoneComponent } from './components/personal-settings/personal-settings-timezone/personal-settings-timezone.component';

import { RefreshPasswordComponent } from './components/refresh-password/refresh-password.component';
import { ConfirmUserService } from './services';

const declarations = [
  ChangePasswordComponent,
  RefreshPasswordComponent,
  EmailConfirmationComponent,
  ForgotPasswordComponent,
  LinkLoggedComponent,
  LoginComponent,
  ManageProfileComponent,
  MySecurityLogsComponent,
  PersonalSettingsComponent,
  ProfilePictureComponent,
  RegisterComponent,
  ResetPasswordComponent,
  SendSecurityCodeComponent,
  PersonalSettingsHalfRowComponent,
  PersonalSettingsEmailComponent,
  PersonalSettingsPhoneNumberComponent,
  PersonalSettingsTimerZoneComponent,
  PasswordComplexityIndicatorComponent,
];

@NgModule({
  declarations: [...declarations],
  exports: [...declarations, PersonalSettingsVerifyButtonComponent],
  imports: [
    CoreModule,
    PasswordComponent,
    CommercialUiModule,
    AccountPublicRoutingModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    NgbPopoverModule,
    NgbDatepickerModule,
    NgbTooltipModule,
    PageModule,
    ShowPasswordDirective,
    TrackCapsLockDirective,
    TwoFactorTabComponent,
    TimeZoneSettingComponent,
    DatetimePickerComponent,
    PersonalSettingsVerifyButtonComponent,
  ],
})
export class AccountPublicModule {
  static forChild(options: AccountConfigOptions): ModuleWithProviders<AccountPublicModule> {
    return {
      ngModule: AccountPublicModule,
      providers: [
        { provide: ACCOUNT_CONFIG_OPTIONS, useValue: options },
        {
          provide: 'ACCOUNT_OPTIONS',
          useFactory: accountOptionsFactory,
          deps: [ACCOUNT_CONFIG_OPTIONS],
        },
        {
          provide: ACCOUNT_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: ACCOUNT_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.personelInfoEntityPropContributors,
        },
        ManageProfileResolver,
        SecurityCodeService,
        RecoveryCodeService,
        ConfirmUserService,
      ],
    };
  }

  static forLazy(options: AccountConfigOptions = {}): NgModuleFactory<AccountPublicModule> {
    return new LazyModuleFactory(AccountPublicModule.forChild(options));
  }
}
