import {
  LPX_RESPONSIVE_BREAKPOINTS_DEFAULTS,
  RESPONSIVE_BREAKPOINTS,
  ResponsiveBreakpoints,
} from '../directives/responsive';
import { Provider } from '@angular/core';

export function createResponsiveProvider(responsiveSettings:ResponsiveBreakpoints |undefined ): Provider{
  return {
    provide: RESPONSIVE_BREAKPOINTS,
    useValue:
      responsiveSettings || LPX_RESPONSIVE_BREAKPOINTS_DEFAULTS,
  };
}
