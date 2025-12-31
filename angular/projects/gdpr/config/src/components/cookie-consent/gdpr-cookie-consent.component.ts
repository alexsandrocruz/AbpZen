import { Component, OnInit, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { CookieConsentService } from '../../services/cookie-consent.service';
import { eGdprConfigComponents } from '../../enums/components';

@Component({
  selector: 'abp-gdpr-cookie-consent',
  templateUrl: './gdpr-cookie-consent.component.html',
  styles: [
    `
      #cookieConsent {
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 999999;
      }
    `,
  ],
})
export class GdprCookieConsentComponent implements OnInit {
  protected readonly document = inject(DOCUMENT);
  protected readonly cookieConsentService = inject(CookieConsentService);

  componentKey = eGdprConfigComponents.CookieConsent;
  cookieKey = '.AspNet.Consent';
  showCookieConsent = true;
  consentOptions$ = this.cookieConsentService.cookieConsentOptions$;

  ngOnInit() {
    this.checkCookieConsent();
  }

  protected checkCookieConsent() {
    const decodedCookies = decodeURIComponent(this.document.cookie);
    this.showCookieConsent = !decodedCookies.includes(`${this.cookieKey}=true`);
  }

  protected acceptCookie() {
    const { expireDate } = this.cookieConsentService.cookieConsentOptions;

    if (!expireDate || typeof expireDate !== 'object') {
      return;
    }

    this.document.cookie = this.cookieKey + '=true;expires=' + expireDate.toUTCString() + ';path=/';
    this.showCookieConsent = false;
  }
}
