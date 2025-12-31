import { InjectionToken } from '@angular/core';

export const LPX_TRANSLATE_KEY_MAP_TOKEN = new InjectionToken<{
  [key: string]: string;
}>('LPX_TRANSLATE_KEY_MAP_TOKEN');
