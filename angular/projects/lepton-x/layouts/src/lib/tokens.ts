import { InjectionToken } from '@angular/core';
import { ContextMenuTriggerEvent, LpxContainer, LpxSelectedLanguageLabelFnType } from './models';
import { SettingsService } from './components/settings';

export const LPX_CONTAINERS = new InjectionToken<LpxContainer[]>(
  'LPX_CONTAINERS'
);

export const LPX_SETTINGS_SERVICE = new InjectionToken<SettingsService>(
  'LPX_SETTINGS_SERVICE'
);

export const LPX_GENERAL_SETTINGS_TRANSLATE_KEY = new InjectionToken<string>(
  'LPX_GENERAL_SETTINGS_TRANSLATE_KEY'
);

export const LPX_SETTINGS_MENU_EVENT_NAME =
  new InjectionToken<ContextMenuTriggerEvent>('LPX_SETTINGS_MENU_EVENT_NAME');

export const LPX_USER_MENU_EVENT_NAME =
  new InjectionToken<ContextMenuTriggerEvent>('LPX_USER_MENU_EVENT_NAME');



export const LPX_SELECTED_LANGUAGE_LABEL_FN = new InjectionToken<LpxSelectedLanguageLabelFnType>('LPX_SELECTED_LANGUAGE_LABEL_FN')