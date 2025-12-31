import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResponsiveBreakpoints } from './directives/responsive/responsive-breakpoints.token';
import { LpxLanguageModuleOptions } from './services/language/models';
import { LpxLanguageModule } from './services/language/language.module';
import { LpxIconModule, LpxIconSettings } from './components/icon/icon.module';
import {
  LpxNavbarModule,
  LpxNavbarSettings,
} from './components/navbar/navbar.module';
import { LpxBreadcrumbModule } from './components/breadcrumb/breadcrumb.module';
import { LpxPerfectScrollbarService, LpxTranslate } from './services';
import { LPX_TRANSLATE_PROVIDERS } from './services/translate/providers';
import { LPX_STYLE_PROVIDERS } from './providers/styles.provider';
import { createWindowProvider } from './providers/window.provider';
import { createResponsiveProvider } from './providers/responsive.provider';
import { createDirectionProvider } from './providers/direction.provider';
import { LpxVisibleDirective } from './directives/visible.directive';
import { LPX_PERFECT_SCROLLBAR } from './tokens';

export interface LpxCoreOptions {
  responsiveSettings?: ResponsiveBreakpoints;
  window?: Window;
  iconSettings?: LpxIconSettings;
  languageSettings?: LpxLanguageModuleOptions;
  navbarSettings?: LpxNavbarSettings;
  texts?: LpxTranslate[];
  listenDirectionChanges?: boolean;
}

@NgModule({
  imports: [CommonModule, LpxVisibleDirective],
})
export class LpxCoreModule {
  static forRoot(options?: LpxCoreOptions): ModuleWithProviders<LpxCoreModule> {
    return {
      ngModule: LpxCoreModule,
      providers: [
        {
          provide: LPX_PERFECT_SCROLLBAR,
          useClass: LpxPerfectScrollbarService,
        },
        createResponsiveProvider(options?.responsiveSettings),
        createWindowProvider(options?.window),
        LpxIconModule.forRoot(options?.iconSettings).providers as Provider,
        LpxLanguageModule.forRoot(options?.languageSettings)
          .providers as Provider[],
        LpxNavbarModule.forRoot(options?.navbarSettings).providers as Provider,
        LpxBreadcrumbModule.forRoot().providers as Provider,
        LPX_TRANSLATE_PROVIDERS,
        ...LPX_STYLE_PROVIDERS,
        createDirectionProvider(options?.listenDirectionChanges || true),
      ],
    };
  }
}
