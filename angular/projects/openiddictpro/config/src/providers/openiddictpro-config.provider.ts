import { makeEnvironmentProviders } from '@angular/core';
import { OPENIDDICT_ROUTE_PROVIDERS } from './';

export function provideOpeniddictproConfig() {
  return makeEnvironmentProviders([OPENIDDICT_ROUTE_PROVIDERS]);
}
