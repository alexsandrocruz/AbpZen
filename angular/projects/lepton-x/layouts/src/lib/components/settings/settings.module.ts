import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxContextMenuModule } from '@volosoft/ngx-lepton-x';
import {
  LpxClickOutsideModule,
  LpxIconModule,
  LpxLanguageModule,
  LpxNavbarModule,
  LpxTranslateModule,
} from '@volo/ngx-lepton-x.core';
import { SettingsComponent } from './settings.component';
import { SettingComponent } from './setting.component';
import { SettingsOptions } from './setting.model';
import {
  LPX_GENERAL_SETTINGS_TRANSLATE_KEY,
  LPX_SETTINGS_MENU_EVENT_NAME,
  LPX_SETTINGS_SERVICE,
} from '../../tokens';

const exportedDeclarations = [SettingsComponent, SettingComponent];

@NgModule({
  declarations: [...exportedDeclarations],
  imports: [
    CommonModule,
    LpxIconModule,
    LpxClickOutsideModule,
    LpxContextMenuModule,
    LpxNavbarModule,
    LpxLanguageModule,
    LpxTranslateModule,
  ],
  exports: [...exportedDeclarations],
})
export class SettingsModule {
  static defineProvidersWith(
    options: SettingsOptions
  ): ModuleWithProviders<SettingsModule> {
    return {
      ngModule: SettingsModule,
      providers: [
        {
          provide: LPX_GENERAL_SETTINGS_TRANSLATE_KEY,
          useValue: options.generalSettingsTranslateKey,
        },
        {
          provide: LPX_SETTINGS_SERVICE,
          useClass: options.settingsService,
        },
        {
          provide: LPX_SETTINGS_MENU_EVENT_NAME,
          useValue: options.settingsMenuEventName || 'click',
        },
      ],
    };
  }
}
