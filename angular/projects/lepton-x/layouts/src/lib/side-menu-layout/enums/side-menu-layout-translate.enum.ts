export enum SideMenuLayoutTranslate {
  SettingsTitle = 'sideMenuLayout.settings.title',
  GeneralSettings = 'sideMenuLayout.settings.generalSettings',
  MenuFilter = 'sideMenuLayout.filterMenu',
  BoxedContainer = 'sideMenuLayout.settings.boxedContainer',
  FullWidthContainer = 'sideMenuLayout.settings.fullWidthContainer',
  FluidContainer = 'sideMenuLayout.settings.fluidContainer',
}

export type SideMenuLayoutTranslateValues = {
  [key in SideMenuLayoutTranslate]: string;
};

export const SideMenuLayoutTranslateDefaults: SideMenuLayoutTranslateValues = {
  [SideMenuLayoutTranslate.SettingsTitle]: 'Container Width',
  [SideMenuLayoutTranslate.GeneralSettings]: 'General Settings',
  [SideMenuLayoutTranslate.MenuFilter]: 'Filter Menu',
  [SideMenuLayoutTranslate.BoxedContainer]: 'Boxed Layout',
  [SideMenuLayoutTranslate.FullWidthContainer]: 'Full Width',
  [SideMenuLayoutTranslate.FluidContainer]: 'Fluid',
};
