import { AuthService, noop } from '@abp/ng.core';
import { APP_INITIALIZER, makeEnvironmentProviders } from '@angular/core';
import { ChatConfigService } from '../services';
import { CHAT_SETTINGS_PROVIDERS, CHAT_ROUTE_PROVIDERS, CHAT_NAV_ITEM_PROVIDERS } from './';

export function provideChatConfig() {
  return makeEnvironmentProviders([
    CHAT_ROUTE_PROVIDERS,
    CHAT_NAV_ITEM_PROVIDERS,
    CHAT_SETTINGS_PROVIDERS,
    {
      provide: APP_INITIALIZER,
      deps: [ChatConfigService, AuthService],
      multi: true,
      useFactory: noop,
    },
  ]);
}
