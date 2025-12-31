import { Component, Inject, Input, OnInit } from '@angular/core';
import { LocalizationService } from '@abp/ng.core';
import { AbpCookieConsentOptions } from '../../models/config-module-options';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'abp-link',
  templateUrl: './link.component.html',
  styles: [],
})
export class LinkComponent implements OnInit {
  @Input() link: AbpCookieConsentOptions;
  splittedArr: string[];
  cookiePolicy: string;
  cookiePolicyUrl: string;
  privacyPolicy: string;
  privacyPolicyUrl: string;
  policyTranslatedText: string[];
  isHrefPrivacy = false;
  isHrefCookie = false;
  window: Window;

  constructor(
    private localizationService: LocalizationService,
    @Inject(DOCUMENT) private document: Document,
  ) {
    this.window = this.document.defaultView;
  }

  ngOnInit(): void {
    this.initLanguage();
    this.changeLanguage();
    this.sameOrigin();
  }

  initLanguage() {
    if (this.link.cookiePolicyUrl && this.link.privacyPolicyUrl) {
      this.cookiePolicyUrl = this.link.cookiePolicyUrl;
      this.privacyPolicyUrl = this.link.privacyPolicyUrl;
      this.cookiePolicy = this.localizationService.instant('AbpGdpr::CookiePolicy');
      this.privacyPolicy = this.localizationService.instant('AbpGdpr::PrivacyPolicy');
      const cookieConsentAgreePolicies = this.localizationService.instant(
        'AbpGdpr::CookieConsentAgreePolicies',
      );
      this.splittedArr = cookieConsentAgreePolicies.split(/["{}01"]+/);
    } else if (this.link.cookiePolicyUrl) {
      this.cookiePolicy = this.localizationService.instant('AbpGdpr::CookiePolicy');
      const cookieConsentAgreePolicy = this.localizationService.instant(
        'AbpGdpr::CookieConsentAgreePolicy',
      );
      this.policyTranslatedText = cookieConsentAgreePolicy.split(/["{}01"]+/);
      this.cookiePolicyUrl = this.link.cookiePolicyUrl;
    } else if (this.link.privacyPolicyUrl) {
      this.privacyPolicy = this.localizationService.instant('AbpGdpr::PrivacyPolicy');
      const cookieConsentAgreePolicy = this.localizationService.instant(
        'AbpGdpr::CookieConsentAgreePolicy',
      );
      this.policyTranslatedText = cookieConsentAgreePolicy.split(/["{}01"]+/);
      this.privacyPolicyUrl = this.link.privacyPolicyUrl;
    }
  }
  changeLanguage() {
    this.localizationService.languageChange$.subscribe(value => {
      if (this.link.cookiePolicyUrl && this.link.privacyPolicyUrl) {
        this.cookiePolicyUrl = this.link.cookiePolicyUrl;
        this.privacyPolicyUrl = this.link.privacyPolicyUrl;
        this.cookiePolicy = this.localizationService.instant('AbpGdpr::CookiePolicy');
        this.privacyPolicy = this.localizationService.instant('AbpGdpr::PrivacyPolicy');
        const cookieConsentAgreePolicies = this.localizationService.instant(
          'AbpGdpr::CookieConsentAgreePolicies',
        );
        this.splittedArr = cookieConsentAgreePolicies.split(/["{}01"]+/);
      } else if (this.link.cookiePolicyUrl) {
        this.cookiePolicy = this.localizationService.instant('AbpGdpr::CookiePolicy');
        const cookieConsentAgreePolicy = this.localizationService.instant(
          'AbpGdpr::CookieConsentAgreePolicy',
        );
        this.policyTranslatedText = cookieConsentAgreePolicy.split(/["{}01"]+/);
        this.cookiePolicyUrl = this.link.cookiePolicyUrl;
      } else if (this.link.privacyPolicyUrl) {
        this.privacyPolicy = this.localizationService.instant('AbpGdpr::PrivacyPolicy');
        const cookieConsentAgreePolicy = this.localizationService.instant(
          'AbpGdpr::CookieConsentAgreePolicy',
        );
        this.policyTranslatedText = cookieConsentAgreePolicy.split(/["{}01"]+/);
        this.privacyPolicyUrl = this.link.privacyPolicyUrl;
      }
    });
  }
  sameOrigin() {
    const siteUrl = this.window.location.host;

    if (this.cookiePolicyUrl) {
      const cookiePolicyUrl = /:\/\/([^\/]+)/.exec(this.cookiePolicyUrl);
      if (cookiePolicyUrl) {
        if (siteUrl !== cookiePolicyUrl[1]) {
          this.isHrefCookie = true;
        }
      }
    }
    if (this.privacyPolicyUrl) {
      const privacyPolicyUrl = /:\/\/([^\/]+)/.exec(this.privacyPolicyUrl);
      if (privacyPolicyUrl) {
        if (siteUrl !== privacyPolicyUrl[1]) {
          this.isHrefPrivacy = true;
        }
      }
    }
  }
}
