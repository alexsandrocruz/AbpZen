import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  authGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { eAccountRouteNames } from '@volo/abp.ng.account/public/config';

import { eAccountComponents } from './enums/components';
import {
  authenticationFlowGuard,
  confirmUserGuard,
  recoveryCodeGuard,
  securityCodeGuard,
} from './guards';

import { accountExtensionsResolver } from './resolvers/extensions.resolver';
import { ManageProfileResolver } from './resolvers/manage-profile.resolver';

import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LinkLoggedComponent } from './components/link-logged/link-logged.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { MySecurityLogsComponent } from './components/my-security-logs/my-security-logs.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { SendSecurityCodeComponent } from './components/send-securiy-code/send-security-code.component';
import { RefreshPasswordComponent } from './components/refresh-password/refresh-password.component';
import { LoginWithRecoveryCodeComponent } from './components/login-with-recovery-code/login-with-recovery-code.component';
import { AccountSessionsComponent } from './components/account-sessions/account-sessions.component';
import { ConfirmUserComponent } from './components/confirm-user/confirm-user.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: '',
    component: RouterOutletComponent,
    resolve: [accountExtensionsResolver],
    children: [
      {
        path: 'login',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.Login,
            defaultComponent: LoginComponent,
          } as ReplaceableComponents.RouteData<LoginComponent>,
        },
        title: 'AbpAccount::Login',
      },
      {
        path: 'register',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.Register,
            defaultComponent: RegisterComponent,
          } as ReplaceableComponents.RouteData<RegisterComponent>,
        },
        title: 'AbpAccount::Register',
      },
      {
        path: 'forgot-password',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.ForgotPassword,
            defaultComponent: ForgotPasswordComponent,
          } as ReplaceableComponents.RouteData<ForgotPasswordComponent>,
        },
        title: 'AbpAccount::ForgotPassword',
      },
      {
        path: 'reset-password',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authenticationFlowGuard],
        data: {
          tenantBoxVisible: false,
          replaceableComponent: {
            key: eAccountComponents.ResetPassword,
            defaultComponent: ResetPasswordComponent,
          } as ReplaceableComponents.RouteData<ResetPasswordComponent>,
        },
        title: 'AbpAccount::ResetPassword',
      },
      {
        path: 'email-confirmation',
        component: ReplaceableRouteContainerComponent,
        data: {
          tenantBoxVisible: false,
          replaceableComponent: {
            key: eAccountComponents.EmailConfirmation,
            defaultComponent: EmailConfirmationComponent,
          } as ReplaceableComponents.RouteData<EmailConfirmationComponent>,
        },
        title: 'AbpAccount::EmailConfirmation',
      },
      {
        path: 'link-logged',
        component: ReplaceableRouteContainerComponent,
        data: {
          tenantBoxVisible: false,
          replaceableComponent: {
            key: eAccountComponents.LinkLogged,
            defaultComponent: LinkLoggedComponent,
          } as ReplaceableComponents.RouteData<LinkLoggedComponent>,
        },
        title: 'AbpAccount::LinkLogged',
      },
      {
        path: 'send-security-code',
        component: ReplaceableRouteContainerComponent,
        canActivate: [securityCodeGuard],
        data: {
          tenantBoxVisible: false,
          replaceableComponent: {
            key: eAccountComponents.SendSecurityCode,
            defaultComponent: SendSecurityCodeComponent,
          } as ReplaceableComponents.RouteData<SendSecurityCodeComponent>,
        },
        title: 'AbpAccount::TwoFactorVerification',
      },
      {
        path: 'login-with-recovery-code',
        component: ReplaceableRouteContainerComponent,
        canActivate: [recoveryCodeGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.LoginWithRecoveryCodeComponent,
            defaultComponent: LoginWithRecoveryCodeComponent,
          } as ReplaceableComponents.RouteData<LoginWithRecoveryCodeComponent>,
        },
        title: 'AbpAccount::LoginWithRecoveryCode',
      },
      {
        path: 'manage',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authGuard],
        resolve: {
          manageProfile: ManageProfileResolver,
        },
        data: {
          replaceableComponent: {
            key: eAccountComponents.ManageProfile,
            defaultComponent: ManageProfileComponent,
          } as ReplaceableComponents.RouteData<ManageProfileComponent>,
        },
        title: 'AbpAccount::MyAccount',
      },
      {
        path: 'security-logs',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.MySecurityLogs,
            defaultComponent: MySecurityLogsComponent,
          } as ReplaceableComponents.RouteData<MySecurityLogsComponent>,
        },
        title: 'AbpAccount::MySecurityLogs',
      },
      {
        path: 'change-password',
        component: ReplaceableRouteContainerComponent,
        data: {
          replaceableComponent: {
            key: eAccountComponents.RefreshPassword,
            defaultComponent: RefreshPasswordComponent,
          } as ReplaceableComponents.RouteData<RefreshPasswordComponent>,
        },
        title: 'AbpAccount::ChangePassword',
      },
      {
        path: 'sessions',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.Sessions,
            defaultComponent: AccountSessionsComponent,
          } as ReplaceableComponents.RouteData<AccountSessionsComponent>,
        },
        title: eAccountRouteNames.Sessions,
      },
      {
        path: 'confirm-user',
        component: ReplaceableRouteContainerComponent,
        canActivate: [confirmUserGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.ConfirmUser,
            defaultComponent: ConfirmUserComponent,
          } as ReplaceableComponents.RouteData<ConfirmUserComponent>,
        },
        title: eAccountRouteNames.ConfirmUser,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountPublicRoutingModule {}
