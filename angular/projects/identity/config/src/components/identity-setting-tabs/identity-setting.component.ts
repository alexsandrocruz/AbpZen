import { ConfigStateService, SubscriptionService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { filter, finalize } from 'rxjs/operators';
import type * as IdentitySettings from '../../models/identity-settings';
import { IdentityConfigService } from '../../services/identity-config.service';

const { required, max, min } = Validators;

@Component({
  selector: 'abp-identity-setting',
  templateUrl: './identity-setting.component.html',
  providers: [SubscriptionService],
})
export class IdentitySettingComponent implements OnInit {
  settings = {} as IdentitySettings.Settings;
  loading: boolean;
  form: UntypedFormGroup;

  doNotRequireConfirmedPhoneNumber = () => {
    this.form.get('signIn.requireConfirmedPhoneNumber').setValue(false);
  };

  enablePhoneNumberConfirmation = () => {
    this.form.get('signIn.enablePhoneNumberConfirmation').setValue(true);
  };

  protected readonly identityConfigService = inject(IdentityConfigService);
  protected readonly toaster = inject(ToasterService);
  protected readonly fb = inject(UntypedFormBuilder);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly configState = inject(ConfigStateService);

  ngOnInit() {
    this.identityConfigService.getSettings().subscribe(settings => {
      this.settings = settings;
      this.buildForm();
      this.syncConfirmationOptions();
    });
  }

  buildForm() {
    const passwordSettings = this.settings?.password || ({} as IdentitySettings.Password);
    const password = this.fb.group({
      requiredLength: [passwordSettings.requiredLength, [required, max(32), min(2)]],
      requiredUniqueChars: [passwordSettings.requiredUniqueChars, [required, max(32), min(1)]],
      requireNonAlphanumeric: passwordSettings.requireNonAlphanumeric,
      requireLowercase: passwordSettings.requireLowercase,
      requireUppercase: passwordSettings.requireUppercase,
      requireDigit: passwordSettings.requireDigit,
      forceUsersToPeriodicallyChangePassword:
        passwordSettings.forceUsersToPeriodicallyChangePassword,
      passwordChangePeriodDays: [passwordSettings.passwordChangePeriodDays, [required, min(0)]],
    });

    const lockoutSettings = this.settings?.lockout || ({} as IdentitySettings.Lockout);

    const lockout = this.fb.group({
      allowedForNewUsers: lockoutSettings.allowedForNewUsers,
      lockoutDuration: [lockoutSettings.lockoutDuration, [required]],
      maxFailedAccessAttempts: [lockoutSettings.maxFailedAccessAttempts, [required]],
    });

    const signInSettings = this.settings?.signIn || ({} as IdentitySettings.SignIn);

    const signIn = this.fb.group({
      requireConfirmedEmail: signInSettings.requireConfirmedEmail,
      enablePhoneNumberConfirmation: signInSettings.enablePhoneNumberConfirmation,
      requireConfirmedPhoneNumber: signInSettings.requireConfirmedPhoneNumber,
    });

    const userSettings = this.settings?.user || ({} as IdentitySettings.User);

    const user = this.fb.group({
      isUserNameUpdateEnabled: userSettings.isUserNameUpdateEnabled,
      isEmailUpdateEnabled: userSettings.isEmailUpdateEnabled,
    });

    this.form = this.fb.group({
      password,
      lockout,
      signIn,
      user,
    });
  }

  syncConfirmationOptions() {
    this.subscription.addOne(
      this.form.get('signIn.requireConfirmedPhoneNumber').valueChanges.pipe(filter(Boolean)),
      this.enablePhoneNumberConfirmation,
    );

    this.subscription.addOne(
      this.form
        .get('signIn.enablePhoneNumberConfirmation')
        .valueChanges.pipe(filter(value => !value)),
      this.doNotRequireConfirmedPhoneNumber,
    );
  }

  submit() {
    if (this.loading || this.form?.invalid) return;

    this.loading = true;
    this.identityConfigService
      .updateSettings(this.form.value)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.configState.refreshAppState().subscribe();
        }),
      )
      .subscribe(() => this.toaster.success('AbpUi::SavedSuccessfully', null));
  }
}
