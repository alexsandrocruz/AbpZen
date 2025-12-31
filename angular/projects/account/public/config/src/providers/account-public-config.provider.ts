import { Injector, makeEnvironmentProviders } from '@angular/core';
import { NAVIGATE_TO_MANAGE_PROFILE } from '@abp/ng.core';
import {
  NAVIGATE_TO_MY_SECURITY_LOGS,
  NAVIGATE_TO_MY_SESSIONS,
} from '@volo/abp.commercial.ng.ui/config';
import { ManageProfileTabsService } from '../services';
import {
  navigateToManageProfileFactory,
  navigateToMySecurityLogsFactory,
  navigateToSessionsFactory,
} from '../utils/factories';
import { ACCOUNT_MANAGE_PROFILE_TAB_PROVIDERS, ACCOUNT_ROUTE_PROVIDERS } from './';

export function provideAccountPublicConfig() {
  return makeEnvironmentProviders([
    ACCOUNT_ROUTE_PROVIDERS,
    ManageProfileTabsService,
    ACCOUNT_MANAGE_PROFILE_TAB_PROVIDERS,
    {
      provide: NAVIGATE_TO_MY_SESSIONS,
      useFactory: navigateToSessionsFactory,
    },
    {
      provide: NAVIGATE_TO_MANAGE_PROFILE,
      useFactory: navigateToManageProfileFactory,
    },
    {
      provide: NAVIGATE_TO_MY_SECURITY_LOGS,
      useFactory: navigateToMySecurityLogsFactory,
      deps: [Injector],
    },
  ]);
}
