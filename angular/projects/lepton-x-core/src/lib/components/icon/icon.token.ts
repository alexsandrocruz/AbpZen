import { InjectionToken } from '@angular/core';

export const ICON_MAP = {
  bagFill: 'bi bi-bag-fill',
  bellFill: 'bi bi-bell-fill',
  calendarWeek: 'bi bi-calendar2-week',
  chatDots: 'bi bi-chat-dots',
  chevronDown: 'bi bi-chevron-down',
  chevronUp: 'bi bi-chevron-up',
  gearConnected: 'bi bi-gear-wide-connected',
  filter: 'bi bi-filter',
  filterFill: 'bi bi-filter-circle-fill',
  layoutThreeColumns: 'bi bi-layout-three-columns',
  moon: 'bi bi-moon',
  square: 'bi bi-square',
  sunset: 'bi bi-brightness-alt-high-fill',
  sunup: 'bi bi-brightness-high-fill',
  star: 'bi bi-star',
  x: 'bi bi-x',
  xCircleFill: 'bi bi-x-circle-fill',
};

export type ICON_MAP_TYPE = typeof ICON_MAP;

export const LEPTON_X_ICON_SET = new InjectionToken<ICON_MAP_TYPE>('LEPTON_X_ICON_SET');
