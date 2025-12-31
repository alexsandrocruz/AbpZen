export enum MobileMenuTranslate {
  SettingsTitle = 'mobileMenu.settings.title',
  MenuTitle = 'mobileMenu.menu.title',
}

export type MobileMenuTranslateValues = {
  [key in MobileMenuTranslate]: string;
};

export const MobileMenuTranslateDefaults: MobileMenuTranslateValues = {
  [MobileMenuTranslate.SettingsTitle]: 'Settings',
  [MobileMenuTranslate.MenuTitle]: 'Menu',
};
