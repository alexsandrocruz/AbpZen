import { InjectionToken } from '@angular/core';
import { LpxStyles } from './models';

export const LPX_INITIAL_STYLES = new InjectionToken<LpxStyles>(
  'LPX_INITIAL_STYLES_TOKEN'
);

export const LPX_STYLE_FINAL = new InjectionToken<LpxStyles>(
  'LPX_STYLE_FINAL_TOKEN'
);

export const LPX_LAYOUT_STYLE_FINAL = new InjectionToken<LpxStyles>(
  'LPX_LAYOUT_STYLE_FINALIZE_TOKEN'
);
