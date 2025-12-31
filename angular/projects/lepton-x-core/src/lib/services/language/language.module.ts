import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxLanguageModuleOptions } from './models';
import { LPX_LANGUAGE } from './tokens';
import { LanguageService } from './language.service';
import { LanguageTranslateDefaults } from './language-translate-keys.enum';
import { LPX_TRANSLATE_TOKEN } from '../translate/tokens';

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class LpxLanguageModule {
  static forRoot(
    options?: LpxLanguageModuleOptions
  ): ModuleWithProviders<LpxLanguageModule> {
    return {
      ngModule: LpxLanguageModule,
      providers: [
        {
          provide: LPX_LANGUAGE,
          useValue: options?.languages || [],
        },
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [LanguageTranslateDefaults],
          multi: true,
        },
        LanguageService,
      ],
    };
  }
}
