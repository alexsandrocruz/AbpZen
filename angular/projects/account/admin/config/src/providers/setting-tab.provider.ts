import { SettingTabsService } from '@abp/ng.setting-management/config';
import { APP_INITIALIZER, inject } from '@angular/core';
import { AccountSettingsComponent } from '@volo/abp.ng.account/admin';
import { eAccountSettingTabNames } from '../enums/setting-tab-names';
import { TimeZoneSettingComponent } from '@volo/abp.ng.account/admin';
import { ConfigStateService } from '@abp/ng.core';
import { ABP } from '@abp/ng.core';
import { filter, firstValueFrom } from 'rxjs';

export const ACCOUNT_SETTING_TAB_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureSettingTabs,
    deps: [SettingTabsService],
    multi: true,
  },
];

export function configureSettingTabs(settingtabs: SettingTabsService) {
  const configState = inject(ConfigStateService);
  const tabsArray: ABP.Tab[] = [
    {
      name: eAccountSettingTabNames.Account,
      order: 100,
      requiredPolicy: 'AbpAccount.SettingManagement',
      component: AccountSettingsComponent,
    },
  ];
  return async () => {
    const kind = await firstValueFrom(configState.getDeep$('clock.kind').pipe(filter(val => val)));

    if (kind === 'Utc') {
      tabsArray.push({
        name: eAccountSettingTabNames.TimeZone,
        order: 100,
        requiredPolicy: 'SettingManagement.TimeZone',
        component: TimeZoneSettingComponent,
      });
    }
    settingtabs.add(tabsArray);
  };
}
