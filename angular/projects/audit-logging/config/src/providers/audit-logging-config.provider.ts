import { makeEnvironmentProviders } from '@angular/core';
import { AUDIT_LOGGING_FEATURES_PROVIDERS } from '@volo/abp.ng.audit-logging/common';
import {
  AUDIT_LOGGING_ROUTE_PROVIDERS,
  AUDIT_LOGGING_SETTING_TAB_PROVIDERS,
  ENTITY_HISTORY_PROVIDERS,
  ENTITY_DETAILS_PROVIDERS,
} from './';

export function provideAuditLoggingConfig() {
  return makeEnvironmentProviders([
    AUDIT_LOGGING_ROUTE_PROVIDERS,
    ENTITY_DETAILS_PROVIDERS,
    ENTITY_HISTORY_PROVIDERS,
    AUDIT_LOGGING_FEATURES_PROVIDERS,
    AUDIT_LOGGING_SETTING_TAB_PROVIDERS,
  ]);
}
