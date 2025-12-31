import { APP_INITIALIZER, ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  createStyleFactory,
  LPX_TRANSLATE_TOKEN,
} from '@volo/ngx-lepton-x.core';
import { ThemeService } from './theme.service';
import { LpxThemeModuleOptions, LpxThemes } from './models';
import { LPX_THEME_DEFAULTS, LPX_THEME_STYLES_DEFAULTS } from './defaults';
import { LPX_THEME_OPTIONS, LPX_THEMES, LPX_THEMES_FINAL } from './tokens';
import { ThemeTranslateDefaults } from './enums';

export function initTheme(themeService: ThemeService) {
  const fn = () => themeService.initTheme();
  return fn;
}

@NgModule({
  imports: [CommonModule],
})
export class LpxThemeModule {
  static forRoot(
    options?: LpxThemeModuleOptions,
    defaultTheme?: string
  ): ModuleWithProviders<LpxThemeModule> {
    if(defaultTheme){
      LPX_THEME_STYLES_DEFAULTS.forEach((theme) => {
        theme.defaultTheme = theme.styleName === defaultTheme;
      });
    }
    return {
      ngModule: LpxThemeModule,
      providers: [
        ThemeService,
        {
          provide: LPX_THEME_OPTIONS,
          useValue: options || LPX_THEME_DEFAULTS,
        },
        {
          provide: LPX_THEMES,
          useValue: LPX_THEME_STYLES_DEFAULTS,
        },
        {
          provide: LPX_THEMES_FINAL,
          deps: [LPX_THEMES],
          useFactory: createStyleFactory<LpxThemes>(options?.styleFactory),
        },
        {
          provide: APP_INITIALIZER,
          useFactory: initTheme,
          deps: [ThemeService],
          multi: true,
        },
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [ThemeTranslateDefaults],
          multi: true,
        },
      ],
    };
  }
}
