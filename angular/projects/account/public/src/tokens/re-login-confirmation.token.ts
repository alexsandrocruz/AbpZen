import { InjectionToken } from '@angular/core';

/**
 * @deprecated
 * Don't need to re-login to application after change personal settings. It'll refresh current user's state.
 */
export const RE_LOGIN_CONFIRMATION_TOKEN = new InjectionToken<boolean>(
  'RE_LOGIN_CONFIRMATION_TOKEN',
);
