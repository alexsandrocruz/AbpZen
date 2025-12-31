import { makeEnvironmentProviders } from '@angular/core';
import { ACCOUNT_SETTING_TAB_PROVIDERS } from './';

export function provideAccountAdminConfig() {
  return makeEnvironmentProviders([ACCOUNT_SETTING_TAB_PROVIDERS]);
}
