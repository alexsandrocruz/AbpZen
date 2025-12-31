import { SettingTabsService } from '@abp/ng.setting-management/config';
import { APP_INITIALIZER } from '@angular/core';
import { eChatModuleTabNames } from '../enums/chat-module-tab-names';
import { ChatTabComponent } from '../components/chat-tab.component';

export const CHAT_SETTINGS_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureSettingTabs,
    deps: [SettingTabsService],
    multi: true,
  },
];

export function configureSettingTabs(settingtabs: SettingTabsService) {
  return () => {
    settingtabs.add([
      {
        name: eChatModuleTabNames.ChatTab,
        order: 100,
        component: ChatTabComponent,
        requiredPolicy:'Chat.Messaging'
      },
    ]);
  };
}
