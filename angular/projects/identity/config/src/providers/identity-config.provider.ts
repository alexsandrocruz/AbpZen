import { noop } from '@abp/ng.core';
import {
  OPEN_MY_LINK_USERS_MODAL,
  OPEN_AUTHORITY_DELEGATION_MODAL,
} from '@volo/abp.commercial.ng.ui/config';
import { APP_INITIALIZER, Injector, makeEnvironmentProviders } from '@angular/core';
import { IDENTITY_ROUTE_PROVIDERS, IDENTITY_SETTING_TAB_PROVIDERS } from './';
import { LinkLoginHandler } from '../handlers/link-login.handler';
import { openMyLinkUsersFactory, openAuthorityDelegationFactory } from '../utils/factories';

export function provideIdentityConfig() {
  return makeEnvironmentProviders([
    IDENTITY_ROUTE_PROVIDERS,
    IDENTITY_SETTING_TAB_PROVIDERS,
    { provide: OPEN_MY_LINK_USERS_MODAL, useFactory: openMyLinkUsersFactory, deps: [Injector] },
    {
      provide: OPEN_AUTHORITY_DELEGATION_MODAL,
      useFactory: openAuthorityDelegationFactory,
      deps: [Injector],
    },
    { provide: APP_INITIALIZER, multi: true, useFactory: noop, deps: [LinkLoginHandler] },
  ]);
}
