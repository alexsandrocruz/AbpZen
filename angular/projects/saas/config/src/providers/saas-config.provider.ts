import { makeEnvironmentProviders } from '@angular/core';
import { SAAS_AUTH_FILTER_PROVIDER, SAAS_ROUTE_PROVIDERS } from './';

export function provideSaasConfig() {
  return makeEnvironmentProviders([SAAS_ROUTE_PROVIDERS, SAAS_AUTH_FILTER_PROVIDER]);
}
