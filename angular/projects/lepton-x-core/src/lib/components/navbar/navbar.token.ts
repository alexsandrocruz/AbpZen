import { InjectionToken, Type } from '@angular/core';
import { LpxNavbarItem } from './models';

export const CONTENT_BEFORE_ROUTES = new InjectionToken<Type<any>[][]>(
  'CONTENT_BEFORE_ROUTES'
);
export const CONTENT_AFTER_ROUTES = new InjectionToken<Type<any>[][]>(
  'CONTENT_AFTER_ROUTES'
);
export const LPX_MENU_ITEMS = new InjectionToken<LpxNavbarItem[]>(
  'LPX_MENU_ITEMS'
);
