import { BundleNames, StyleNames, ThemeTranslateKeys } from './enums';
import { LpxTheme, LpxThemeOptions, LpxThemes } from './models';

export const dimTheme = new LpxTheme({
  bundles: [
    {
      bundleName: BundleNames.BootstrapDim,
    },
    {
      bundleName: BundleNames.Dim,
    },
  ],
  styleName: StyleNames.Dim,
  icon: 'bi bi-brightness-alt-high-fill',
  label: ThemeTranslateKeys.SemiDarkMode,
});

export const darkTheme = new LpxTheme({
  bundles: [
    {
      bundleName: BundleNames.BootstrapDark,
    },
    {
      bundleName: BundleNames.Dark,
    },
  ],
  styleName: StyleNames.Dark,
  systemKey: StyleNames.Dark,
  icon: 'bi bi-moon-fill',
  label: ThemeTranslateKeys.DarkMode,
});

export const lightTheme = new LpxTheme({
  bundles: [
    {
      bundleName: BundleNames.BootstrapLight,
    },
    {
      bundleName: BundleNames.Light,
    },
  ],
  systemKey: StyleNames.Light,
  styleName: StyleNames.Light,
  icon: 'bi bi-sun-fill',
  label: ThemeTranslateKeys.LightMode,
});

export const LPX_THEME_DEFAULTS: LpxThemeOptions = {
  localStorageKey: 'LPX_THEME',
  disableSystemOption: false,
};
export const LPX_THEME_STYLES_DEFAULTS: LpxThemes = [
  lightTheme,
  dimTheme,
  darkTheme,
];
export const ThemeLinkElemId = 'lpxTheme';
