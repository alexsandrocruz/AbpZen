import { Provider } from '@angular/core';
import {
  LanguageTranslateKeys,
  LanguageTranslateValues,
  LPX_TRANSLATE_SERVICE_TOKEN,
} from '@volo/ngx-lepton-x.core';
import { AbpTranslateService } from '../services/abp-translate.service';
import { LPX_TRANSLATE_KEY_MAP_TOKEN } from '../tokens/translate-key-map';
import {
  ThemeTranslateKeys,
  ThemeTranslateValues,
} from '@volosoft/ngx-lepton-x';

export const LPX_TRANSLATE_PROVIDER: Provider = {
  provide: LPX_TRANSLATE_SERVICE_TOKEN,
  useClass: AbpTranslateService,
};
export const translateKeys: ThemeTranslateValues & LanguageTranslateValues = {
  [LanguageTranslateKeys.SettingsTitle]: 'LeptonX::Language',
  [ThemeTranslateKeys.AppearanceTitle]: 'LeptonX::Appearance',
  [ThemeTranslateKeys.DarkMode]: 'LeptonX::Theme:dark',
  [ThemeTranslateKeys.LightMode]: 'LeptonX::Theme:light',
  [ThemeTranslateKeys.SemiDarkMode]: 'LeptonX::Theme:dim',
  [ThemeTranslateKeys.System]: 'LeptonX::Theme:system',
};
export const LPX_TRANSLATE_KEYS_PROVIDER: Provider = {
  provide: LPX_TRANSLATE_KEY_MAP_TOKEN,
  multi: true,
  useValue: translateKeys,
};
