import { APP_INITIALIZER, Provider } from '@angular/core';
import { LanguageService } from '../services/language';
import { StyleService } from '../style';
import {
  distinctUntilKeyChanged,
  filter,
  switchMap,
  take,
} from 'rxjs/operators';

export function createDirectionProvider(listenDirection: boolean): Provider {
  return {
    provide: APP_INITIALIZER,
    multi: true,
    deps: [LanguageService, StyleService],
    useFactory: listenDirection ? listenDirectionChange : () => () => null,
  };
}
// subscribe to direction from documentElement and load direction stylesheet
export function listenDirectionChange(
  languageService: LanguageService,
  styleService: StyleService
) {
  return () => {
    return new Promise((resolve) => {
      styleService.initialized$
        .pipe(
          filter(Boolean),
          take(1),
          switchMap(() => languageService.languageChange$),
          distinctUntilKeyChanged('isRTL')
        )
        .subscribe(async (lang) => {
          const direction = lang?.isRTL ? 'rtl' : 'ltr';
          const documentElement = document.documentElement;
          if (documentElement.dir !== direction) {
            documentElement.dir = direction;
          }
          await styleService.reloadInitialStyles(direction);
          resolve(null);
        });
    });
  };
}
