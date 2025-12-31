import { APP_INITIALIZER, inject } from '@angular/core';
import { map } from 'rxjs';
import {
  AuthService,
  ConfigStateService,
  EnvironmentService,
  NAVIGATE_TO_MANAGE_PROFILE,
} from '@abp/ng.core';
import { UserMenuService } from '@abp/ng.theme.shared';
import {
  NAVIGATE_TO_MY_SESSIONS,
  NAVIGATE_TO_MY_SECURITY_LOGS,
  OPEN_MY_LINK_USERS_MODAL,
  OPEN_AUTHORITY_DELEGATION_MODAL,
  NAVIGATE_TO_MY_EXTERNAL_LOGINS,
} from '@volo/abp.commercial.ng.ui/config';
import { eUserMenuItems } from '../enums/user-menu-items';

export const LEPTON_X_USER_MENU_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    multi: true,
    useFactory: configureUserMenu,
  },
];

export function configureUserMenu() {
  const userMenu = inject(UserMenuService);
  const authService = inject(AuthService);
  const configState = inject(ConfigStateService);
  const environment = inject(EnvironmentService);

  const navigateToMySessions = inject(NAVIGATE_TO_MY_SESSIONS);
  const navigateToMyExternalLogins = inject(NAVIGATE_TO_MY_EXTERNAL_LOGINS);
  const navigateToManageProfile = inject(NAVIGATE_TO_MANAGE_PROFILE);
  const navigateToMySecurityLogs = inject(NAVIGATE_TO_MY_SECURITY_LOGS);

  const openMyLinkUsersModal = inject(OPEN_MY_LINK_USERS_MODAL, {
    optional: true,
  }) as () => void;

  const openAuthorityDelegationModal = inject(OPEN_AUTHORITY_DELEGATION_MODAL, {
    optional: true,
  }) as () => void;

  return () => {
    userMenu.addItems([
      {
        id: eUserMenuItems.Sessions,
        order: 100,
        textTemplate: {
          icon: 'bi bi-clock-fill',
          text: 'AbpAccount::Sessions',
        },
        action: () => navigateToMySessions(),
      },
      {
        id: eUserMenuItems.ExternalLogins,
        order: 101,
        textTemplate: {
          icon: 'bi bi-person-circle',
          text: 'AbpAccount::ExternalLogins',
        },
        action: () => navigateToMyExternalLogins(),
        visible: () => {
          return environment.getEnvironment$().pipe(
            map(({ oAuthConfig }) => {
              return oAuthConfig?.responseType === 'code';
            }),
          );
        },
      },
      {
        id: eUserMenuItems.LinkedAccounts,
        order: 102,
        textTemplate: {
          icon: 'bi bi-link',
          text: 'AbpAccount::LinkedAccounts',
        },
        action: () => openMyLinkUsersModal(),
        visible: () => !!openMyLinkUsersModal,
      },
      {
        id: eUserMenuItems.AuthorityDelegation,
        order: 103,
        textTemplate: {
          text: 'AbpAccount::AuthorityDelegation',
          icon: 'fa fa-users',
        },
        visible: () => {
          return configState
            .getOne$('currentUser')
            .pipe(
              map(({ impersonatorUserId }) => !Boolean(impersonatorUserId)),
            );
        },
        action: () => openAuthorityDelegationModal(),
      },
      {
        id: eUserMenuItems.MyAccount,
        order: 104,
        textTemplate: {
          icon: 'bi bi-sliders',
          text: 'AbpAccount::MyAccount',
        },
        action: () => navigateToManageProfile(),
      },
      {
        id: eUserMenuItems.SecurityLogs,
        order: 105,
        textTemplate: {
          icon: 'bi bi-list-ul',
          text: 'AbpAccount::MySecurityLogs',
        },
        action: () => navigateToMySecurityLogs(),
      },
      {
        id: eUserMenuItems.Logout,
        order: 106,
        textTemplate: {
          icon: 'bi bi-box-arrow-right',
          text: 'AbpUi::Logout',
        },
        action: () => {
          authService.logout().subscribe();
        },
      },
    ]);
  };
}
