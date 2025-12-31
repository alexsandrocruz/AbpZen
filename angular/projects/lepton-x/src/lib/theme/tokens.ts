import { InjectionToken } from '@angular/core';
import { LpxThemeOptions, LpxThemeStyleConfig } from './models';

export const LPX_THEME_OPTIONS = new InjectionToken<LpxThemeOptions>(
  'LPX_THEME_SETTINGS'
);

export const LPX_THEMES = new InjectionToken<LpxThemeStyleConfig[]>(
  'LPX_THEMES'
);

export const LPX_THEMES_FINAL = new InjectionToken<LpxThemeStyleConfig[]>(
  'LPX_THEMES_FINAL'
);
