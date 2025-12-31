import { Injectable } from '@angular/core';
import { InternalStore } from '@abp/ng.core';
import { AbpCookieConsentOptions } from '../models/config-module-options';

@Injectable({
  providedIn: 'root',
})
export class CookieConsentService {
  readonly #store = new InternalStore({} as AbpCookieConsentOptions);

  readonly cookieConsentOptions$ = this.#store.sliceState(state => state);

  get cookieConsentOptions() {
    return this.#store.state;
  }

  setOptions(options: AbpCookieConsentOptions) {
    this.#store.patch({
      isEnabled: options.isEnabled,
      cookiePolicyUrl: options.cookiePolicyUrl,
      privacyPolicyUrl: options.privacyPolicyUrl,
      expireDate: options.expireDate,
    });
  }
}
