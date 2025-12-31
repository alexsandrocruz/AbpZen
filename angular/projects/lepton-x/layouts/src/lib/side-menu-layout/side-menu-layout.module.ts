import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  LpxContextMenuModule,
  LpxThemeModule,
  LpxTopbarContentModule,
} from '@volosoft/ngx-lepton-x';
import {
  LPX_AUTH_SERVICE_PROVIDER,
  LPX_TRANSLATE_TOKEN,
  LpxAvatarModule,
  LpxBrandLogoModule,
  LpxBreadcrumbModule,
  LpxFooterModule,
  LpxNavbarModule,
  LpxResponsiveModule,
  PanelsModule,
  PerfectScrollbarDirective,
} from '@volo/ngx-lepton-x.core';

import { LpxMobileNavbarModule } from '../components/mobile-navbar/mobile-navbar.module';
import { LpxToolbarContainerModule } from '../components/toolbar-container/toolbar-container.module';
import { LpxSideMenuLayoutOptions } from '../models';
import { LPX_CONTAINERS } from '../tokens';
import { SideMenuLayoutComponent } from './side-menu-layout.component';
import { SideMenuContainerDefaults } from './defaults';
import { MenuFilterComponent, MenuFilterModule } from './menu-filter';
import {
  SideMenuLayoutTranslate,
  SideMenuLayoutTranslateDefaults,
} from './enums';
import { getLpxSideMenuLayoutStyleProviders } from './providers';
import { SettingsModule } from '../components';
import { SidebarSettingsService } from './services';

const declarationsWithExports = [SideMenuLayoutComponent];

const usedModules = [
  LpxTopbarContentModule,
  LpxBreadcrumbModule,
  SettingsModule,
  LpxMobileNavbarModule,
  LpxNavbarModule,
  LpxToolbarContainerModule,
  MenuFilterModule,
  PanelsModule,
  LpxFooterModule,
  LpxBrandLogoModule,
  LpxAvatarModule,
  LpxContextMenuModule,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [
    CommonModule,
    ...usedModules,
    LpxResponsiveModule,
    PerfectScrollbarDirective,
    LpxThemeModule,
  ],
  exports: [...declarationsWithExports, ...usedModules],
})
export class LpxSideMenuLayoutModule {
  static forRoot(
    options?: LpxSideMenuLayoutOptions,
  ): ModuleWithProviders<LpxSideMenuLayoutModule> {
    return {
      ngModule: LpxSideMenuLayoutModule,
      providers: [
        {
          provide: LPX_CONTAINERS,
          useValue: options?.containers || SideMenuContainerDefaults,
        },
        LpxNavbarModule.forChild({
          contentBeforeRoutes: [MenuFilterComponent],
        }).providers as Provider,
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [SideMenuLayoutTranslateDefaults],
          multi: true,
        },
        LPX_AUTH_SERVICE_PROVIDER,
        getLpxSideMenuLayoutStyleProviders(options?.styleFactory),
        LpxToolbarContainerModule.forRoot().providers as Provider,
        LpxMobileNavbarModule.forRoot().providers as Provider,
        SettingsModule.defineProvidersWith({
          generalSettingsTranslateKey: SideMenuLayoutTranslate.GeneralSettings,
          settingsService: SidebarSettingsService,
          settingsMenuEventName: options?.settingsMenuEventName || 'click',
        }).providers as Provider,
      ],
    };
  }
}
