import {
  MobileMenuTranslate,
  MobileMenuTranslateValues,
  SideMenuLayoutTranslate,
  SideMenuLayoutTranslateValues,
  ToolbarTranslateKeys,
  ToolbarTranslateValues,
} from '@volosoft/ngx-lepton-x/layouts';
import { Provider } from '@angular/core';
import { LPX_TRANSLATE_KEY_MAP_TOKEN } from '@volosoft/abp.ng.theme.lepton-x';

const keyMap: SideMenuLayoutTranslateValues &
  MobileMenuTranslateValues &
  ToolbarTranslateValues = {
  [SideMenuLayoutTranslate.GeneralSettings]: 'LeptonX::GeneralSettings',
  [SideMenuLayoutTranslate.MenuFilter]: 'LeptonX::FilterMenu',
  [SideMenuLayoutTranslate.SettingsTitle]: 'LeptonX::ContainerWidth',
  [SideMenuLayoutTranslate.BoxedContainer]: 'LeptonX::ContainerWidth:Boxed',
  [SideMenuLayoutTranslate.FullWidthContainer]:
    'LeptonX::ContainerWidth:FullWidth',
  [SideMenuLayoutTranslate.FluidContainer]: 'LeptonX::ContainerWidth:Fluid',
  [MobileMenuTranslate.SettingsTitle]: 'LeptonX::Settings',
  [MobileMenuTranslate.MenuTitle]: 'LeptonX::Menu',
  [ToolbarTranslateKeys.ContextMenuWelcome]: 'LeptonX::Welcome',
};

export const LPX_TRANSLATE_KEYS_PROVIDER: Provider = {
  provide: LPX_TRANSLATE_KEY_MAP_TOKEN,
  multi: true,
  useValue: keyMap,
};
