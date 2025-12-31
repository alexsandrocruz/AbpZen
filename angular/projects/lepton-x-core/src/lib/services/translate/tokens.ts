import { InjectionToken } from '@angular/core';
import { LpxTranslate, TranslateService } from './models';

export const LPX_TRANSLATE_SERVICE_TOKEN = new InjectionToken<TranslateService>(
  'LPX_TRANSLATE_SERVICE_TOKEN'
);
export const LPX_TRANSLATE_TOKEN = new InjectionToken<LpxTranslate[]>(
  'LPX_TRANSLATE_TOKEN'
);
