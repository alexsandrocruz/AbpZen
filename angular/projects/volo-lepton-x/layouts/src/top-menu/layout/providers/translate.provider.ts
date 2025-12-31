import {
  MobileMenuTranslate,
  MobileMenuTranslateValues,
  TopMenuLayoutTranslate,
  TopMenuLayoutTranslateValues,
  ToolbarTranslateKeys,
  ToolbarTranslateValues,
} from '@volosoft/ngx-lepton-x/layouts';
import { Provider } from '@angular/core';
import { LPX_TRANSLATE_KEY_MAP_TOKEN } from '@volosoft/abp.ng.theme.lepton-x';

const keyMap: TopMenuLayoutTranslateValues &
  MobileMenuTranslateValues &
  ToolbarTranslateValues = {
  [TopMenuLayoutTranslate.GeneralSettings]: 'LeptonX::GeneralSettings',
  [MobileMenuTranslate.SettingsTitle]: 'LeptonX::Settings',
  [MobileMenuTranslate.MenuTitle]: 'LeptonX::Menu',
  [ToolbarTranslateKeys.ContextMenuWelcome]: 'LeptonX::Welcome',
};

export const LPX_TRANSLATE_KEYS_PROVIDER: Provider = {
  provide: LPX_TRANSLATE_KEY_MAP_TOKEN,
  multi: true,
  useValue: keyMap,
};
