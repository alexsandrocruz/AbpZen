import { Injectable } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { ThemeService } from '@volosoft/ngx-lepton-x';
import { LanguageService } from '@volo/ngx-lepton-x.core';
import { map } from 'rxjs/operators';
import { SettingsService } from '../../../components/settings/settings-service.model';
import { Setting } from '../../../components/settings/setting.model';
import { combineAndFilterByChildren } from '../../../utils/stream-utils';

@Injectable({
  providedIn: 'root',
})
export class TopBarSettingsService implements SettingsService {
  settings$: Observable<Setting[]> = combineAndFilterByChildren([
    this.themeService.stylesAsSettingsGroup$,
    this.languageService.languagesAsSettingsGroup$,
  ]);

  selectedSettings$ = combineLatest([
    this.themeService.selectedStyle$.pipe(
      map((style) => ({
        icon: style.icon,
        id: this.themeService.id,
      }))
    ),
    this.languageService.selectedLanguage$.pipe(
      map((lang) => ({
        shortText: lang?.cultureName?.toLocaleUpperCase(),
        id: this.languageService.id,
      }))
    ),
  ]);

  constructor(
    private themeService: ThemeService,
    private languageService: LanguageService
  ) {}
}
