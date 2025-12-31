import { Provider } from '@angular/core';
import { LPX_TRANSLATE_SERVICE_TOKEN } from './tokens';
import { DefaultTranslateService } from './default-translate.service';

export const LPX_TRANSLATE_SERVICE_PROVIDER: Provider = {
  provide: LPX_TRANSLATE_SERVICE_TOKEN,
  useClass: DefaultTranslateService,
};

export const LPX_TRANSLATE_PROVIDERS: Provider[] = [
  LPX_TRANSLATE_SERVICE_PROVIDER,
];
