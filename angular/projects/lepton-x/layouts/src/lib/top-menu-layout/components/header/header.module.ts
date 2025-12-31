import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import {
  LpxAvatarModule,
  LpxBrandLogoModule,
  LpxClickOutsideModule,
  LpxNavbarModule,
} from '@volo/ngx-lepton-x.core';
import { LpxToolbarModule, SettingsModule } from '../../../components';
import { TopMenuLayoutTranslate } from '../../enums/top-menu-layout-translate.enum';
import { TopBarSettingsService } from './topbar-settings.service';
import { TopMenuNavItemModule } from './top-menu-nav-item';
import { TopMenuNavbarComponent } from './top-menu-navbar/top-menu-navbar.component';
import { HeaderUserComponent } from './header-user/header-user.component';
import { LpxContextMenuModule } from '@volosoft/ngx-lepton-x';
import { HeaderToolbarComponent } from './header-toolbar/header-toolbar.component';
import { ContextMenuTriggerEvent } from '../../../models';
import { LPX_USER_MENU_EVENT_NAME } from '../../../tokens';

@NgModule({
  declarations: [
    HeaderComponent,
    TopMenuNavbarComponent,
    HeaderToolbarComponent,
    HeaderUserComponent,
  ],
  imports: [
    CommonModule,
    LpxBrandLogoModule,
    SettingsModule,
    TopMenuNavItemModule,
    LpxAvatarModule,
    LpxToolbarModule,
    LpxContextMenuModule,
    LpxNavbarModule,
    LpxClickOutsideModule,
  ],
  exports: [HeaderComponent, TopMenuNavbarComponent],
})
export class LpxHeaderModule {
  static forRoot(
    userMenuEventName: ContextMenuTriggerEvent,
    settingsMenuEventName: ContextMenuTriggerEvent
  ): ModuleWithProviders<LpxHeaderModule> {
    return {
      ngModule: LpxHeaderModule,
      providers: [
        {
          provide: LPX_USER_MENU_EVENT_NAME,
          useValue: userMenuEventName,
        },
        SettingsModule.defineProvidersWith({
          generalSettingsTranslateKey: TopMenuLayoutTranslate.GeneralSettings,
          settingsService: TopBarSettingsService,
          settingsMenuEventName: settingsMenuEventName,
        }).providers as Provider,
      ],
    };
  }
}
