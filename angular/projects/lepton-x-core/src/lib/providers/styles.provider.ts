import { APP_INITIALIZER, Provider } from '@angular/core';
import { LPX_INITIAL_STYLES, StyleService } from '../style';
import { LanguageService } from '../services';
import { switchMap, take } from 'rxjs/operators';
import { Observable, from } from 'rxjs';

export const LPX_STYLE_PROVIDERS: Provider[] = [
  {
    provide: LPX_INITIAL_STYLES,
    useFactory: () => [],
  },
  {
    provide: APP_INITIALIZER,
    deps: [StyleService, LanguageService],
    useFactory: loadInitialStyles,
    multi: true,
  },
];

export function loadInitialStyles(
  styleService: StyleService,
  languageService: LanguageService
): () => Observable<any> {
  return () => {
    return languageService.languageChange$.pipe(
      take(1),
      switchMap((lang) =>
        from(styleService.initStyles(lang.isRTL ? 'rtl' : 'ltr'))
      )
    );
  };
}
