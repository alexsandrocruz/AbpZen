import { APP_INITIALIZER } from '@angular/core';
import { eAccountManageProfileTabNames } from '../enums/manage-profile-tab-names';
import { ManageProfileTabsService } from '../services/manage-profile-tabs.service';

export const ACCOUNT_MANAGE_PROFILE_TAB_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureTabs,
    deps: [ManageProfileTabsService],
    multi: true,
  },
];

export function configureTabs(tabs: ManageProfileTabsService) {
  return () => {
    tabs.add([
      {
        name: eAccountManageProfileTabNames.ProfilePicture,
        order: 1,
        component: null,
      },
      {
        name: eAccountManageProfileTabNames.ChangePassword,
        order: 2,
        component: null,
      },
      {
        name: eAccountManageProfileTabNames.PersonalInfo,
        order: 3,
        component: null,
      },
      {
        name: eAccountManageProfileTabNames.AuthenticatorApp,
        order: 4,
        component: null,
      },
      {
        name: eAccountManageProfileTabNames.TwoFactor,
        order: 5,
        component: null,
      },
    ]);
  };
}
