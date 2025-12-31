import { APP_INITIALIZER, Provider } from '@angular/core';
import { Router } from '@angular/router';
import { UserMenuService } from '@abp/ng.theme.shared';

export const GDPR_USER_MENU_ITEM_PROVIDERS: Provider[] = [
  {
    provide: APP_INITIALIZER,
    multi: true,
    deps: [UserMenuService, Router],
    useFactory: addToUserMenu,
  },
];

export function addToUserMenu(userMenu: UserMenuService, router: Router) {
  return () => {
    userMenu.addItems([
      {
        id: 'Gdpr.GdprNavigation',
        order: 100,
        textTemplate: {
          text: 'AbpGdpr::Menu:PersonalData',
          icon: 'fa fa-lock',
        },
        action: () => router.navigateByUrl('/gdpr'),
      },
    ]);
  };
}
