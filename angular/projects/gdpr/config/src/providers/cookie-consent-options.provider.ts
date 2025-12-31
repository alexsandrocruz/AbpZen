import { APP_INITIALIZER, Provider, inject } from '@angular/core';
import { AbpCookieConsentOptions } from '../models/config-module-options';
import { CookieConsentService } from '../services';

export function cookieConsentOptionsProvider(options: AbpCookieConsentOptions = {}): Provider {
  return {
    provide: APP_INITIALIZER,
    multi: true,
    useFactory: () => {
      const cookieConsentService = inject(CookieConsentService);

      const defaultExpireDate = new Date(
        new Date().getFullYear(),
        new Date().getMonth() + 6,
        new Date().getDate(),
      );
      return () => {
        cookieConsentService.setOptions({
          isEnabled: options.isEnabled || true,
          expireDate: options.expireDate || defaultExpireDate,
          ...options,
        });
      };
    },
  };
}
