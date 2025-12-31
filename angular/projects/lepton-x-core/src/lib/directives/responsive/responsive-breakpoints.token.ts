import { InjectionToken } from '@angular/core';

export interface ResponsiveBreakpoints {
  [key: string]: number;
}

export const RESPONSIVE_BREAKPOINTS = new InjectionToken<ResponsiveBreakpoints>(
  'RESPONSIVE_BREAKPOINTS',
);
