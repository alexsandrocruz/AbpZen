import { ConfigStateService, SessionStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { eTwoFactorBehaviour } from '../enums/two-factor-behaviour';
import { AccountCaptchaService } from '../services/account-captcha.service';
import { AccountExternalProviderService } from '../services/account-external-provider.service';

@Component({
  selector: 'abp-account-settings',
  templateUrl: './account-settings.component.html',
  providers: [AccountExternalProviderService, AccountCaptchaService],
})
export class AccountSettingsComponent implements OnInit {
  isTwoFactorSettingsEnabled: boolean;
  isExternalProviderEnabled$: Observable<boolean>;
  isExternalProviderExists$: Observable<boolean>;
  isCaptchaEnabled$: Observable<boolean>;
  isTenant: boolean;

  constructor(
    private configStateService: ConfigStateService,
    private sessionStateService: SessionStateService,
    private captchaService: AccountCaptchaService,
    private externalProviderService: AccountExternalProviderService,
  ) {}

  ngOnInit() {
    this.isTwoFactorSettingsEnabled =
      this.configStateService.getFeature('Identity.TwoFactor') ===
      eTwoFactorBehaviour[eTwoFactorBehaviour.Optional];

    this.isExternalProviderExists$ = this.externalProviderService
      .getSettings()
      .pipe(map(data => data?.externalProviders?.length > 0));

    this.isTenant = this.sessionStateService.getTenant()?.isAvailable;

    if (this.isTenant) {
      this.isExternalProviderEnabled$ = this.externalProviderService
        .getSettings()
        .pipe(map(result => result.externalProviders.some(settings => settings.enabled)));

      this.isCaptchaEnabled$ = this.captchaService
        .getSettings()
        .pipe(map(result => result.useCaptchaOnLogin || result.useCaptchaOnRegistration));
    } else {
      this.isExternalProviderEnabled$ = of(true);
      this.isCaptchaEnabled$ = of(true);
    }
  }
}
