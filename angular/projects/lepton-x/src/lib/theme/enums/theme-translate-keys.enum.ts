export enum ThemeTranslateKeys {
  AppearanceTitle = 'theme.settings.title',
  DarkMode = 'theme.settings.darkMode',
  LightMode = 'theme.settings.lightMode',
  SemiDarkMode = 'theme.settings.semiDarkMode',
  System = 'theme.settings.system',
}

export type ThemeTranslateValues = {
  [key in ThemeTranslateKeys]: string;
};

export const ThemeTranslateDefaults: ThemeTranslateValues = {
  [ThemeTranslateKeys.AppearanceTitle]: 'Appearance',
  [ThemeTranslateKeys.DarkMode]: 'Dark',
  [ThemeTranslateKeys.LightMode]: 'Light',
  [ThemeTranslateKeys.SemiDarkMode]: 'Semi-Dark',
  [ThemeTranslateKeys.System]: 'System',
};
