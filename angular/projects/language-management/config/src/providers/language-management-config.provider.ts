import { makeEnvironmentProviders } from '@angular/core';
import { LANGUAGE_MANAGEMENT_FEATURES_PROVIDERS } from '@volo/abp.ng.language-management/common';
import { LANGUAGE_MANAGEMENT_ROUTE_PROVIDERS } from './';

export function provideLanguageManagementConfig() {
  return makeEnvironmentProviders([
    LANGUAGE_MANAGEMENT_ROUTE_PROVIDERS,
    LANGUAGE_MANAGEMENT_FEATURES_PROVIDERS,
  ]);
}
