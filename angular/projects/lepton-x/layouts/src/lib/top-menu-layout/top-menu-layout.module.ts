import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxTopbarContentModule } from '@volosoft/ngx-lepton-x';
import {
  LPX_AUTH_SERVICE_PROVIDER,
  LPX_TRANSLATE_TOKEN,
  LpxBreadcrumbModule,
  LpxLanguageModule,
  LpxNavbarModule,
  PanelsModule,
  LpxFooterModule,
  LpxResponsiveModule,
} from '@volo/ngx-lepton-x.core';
import { LpxMobileNavbarModule } from '../components/mobile-navbar/mobile-navbar.module';
import { TopMenuLayoutComponent } from './top-menu-layout.component';
import { LpxToolbarContainerModule } from '../components';
import { LpxHeaderModule } from './components/header/header.module';
import { getLpxTopMenuLayoutStyleProviders } from './providers';
import { TopMenuSubNavbarService } from './services';
import { LpxTopMenuLayoutOptions } from '../models';
import { TopMenuLayoutTranslateDefaults } from './enums';



const declarationsWithExports = [TopMenuLayoutComponent];
const usedModules = [
  LpxTopbarContentModule,
  LpxBreadcrumbModule,
  LpxMobileNavbarModule,
  LpxHeaderModule,
  LpxLanguageModule,
  PanelsModule,
  LpxResponsiveModule,
  LpxFooterModule,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule, ...usedModules],
  providers: [],
  exports: [...declarationsWithExports, ...usedModules],
})
export class LpxTopMenuLayoutModule {
  static forRoot(
    options?: LpxTopMenuLayoutOptions
  ): ModuleWithProviders<LpxTopMenuLayoutModule> {
    return {
      ngModule: LpxTopMenuLayoutModule,
      providers: [


        LpxNavbarModule.forChild().providers as Provider,

        LPX_AUTH_SERVICE_PROVIDER,
        getLpxTopMenuLayoutStyleProviders(options?.styleFactory),
        LpxToolbarContainerModule.forRoot().providers as Provider,
        LpxMobileNavbarModule.forRoot().providers as Provider,
        TopMenuSubNavbarService,
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [TopMenuLayoutTranslateDefaults],
          multi: true,
        },
        LpxHeaderModule.forRoot(
          options?.userMenuEventName || 'hover',
          options?.settingsMenuEventName || 'hover'
        ).providers as Provider,
      ],
    };
  }
}
