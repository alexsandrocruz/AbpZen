import { LpxLanguage, LpxNavbarItem, LpxStyleFactory } from '@volo/ngx-lepton-x.core';

export interface LpxContainer {
  layoutClass: string;
  label: string;
  icon: string;
  order?: number;
  default?: boolean;
}

export interface LpxSideMenuLayoutOptions {
  containers?: LpxContainer[];
  styleFactory?: LpxStyleFactory;
  settingsMenuEventName?: ContextMenuTriggerEvent;
}

export interface LpxTopMenuLayoutOptions {
  styleFactory?: LpxStyleFactory;
  topMenuItems?: LpxNavbarItem[];
  userMenuEventName?: ContextMenuTriggerEvent;
  settingsMenuEventName?: ContextMenuTriggerEvent;
}

export type ContextMenuTriggerEvent = 'click' | 'hover';

export type LpxSelectedLanguageLabelFnType  = (lang: LpxLanguage | undefined, id:string | undefined) => {shortText:string | undefined, id:string | undefined};
