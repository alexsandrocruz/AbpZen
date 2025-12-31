import { Inject, Injectable } from '@angular/core';
import { distinctUntilChanged, filter, map } from 'rxjs/operators';
import { DataStore } from '../../utils/data-store';
import { LPX_LANGUAGE } from './tokens';
import { LpxNavbarItem } from '../../components/navbar/models';
import { LpxLanguage, LpxLanguageState } from './models';
import { LanguageTranslateKeys } from './language-translate-keys.enum';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  readonly store = new DataStore<LpxLanguageState>({ languages: [] });

  id = 'languages';

  convertLanguageToNavbarItem = (languages: LpxLanguage[]): LpxNavbarItem[] => {
    return languages.map((lang) => ({
      icon: '',
      text: lang.displayName,
      selected: lang.selected,
      action: () => {
        this.setSelectedLanguage(lang);
        return true;
      },
    }));
  };

  selectedLanguage$ = this.store.sliceState(
    ({ selectedLanguage }) => selectedLanguage
  );

  get selectedLanguage(): LpxLanguage {
    return this.store.state.selectedLanguage as LpxLanguage;
  }

  languageChange$: Observable<LpxLanguage> = this.selectedLanguage$.pipe(
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    filter<LpxLanguage>((lang: LpxLanguage) => lang !== undefined),
    distinctUntilChanged((a, b) => a?.cultureName === b?.cultureName)
  );
  languages$ = this.store.sliceState((state) => state.languages);
  languagesAsNavbarItems$ = this.languages$.pipe(
    map(this.convertLanguageToNavbarItem)
  );
  //TODO: PROVIDE API
  languagesAsSettingsGroup$ = this.languagesAsNavbarItems$.pipe(
    map((languages) => ({
      text: LanguageTranslateKeys.SettingsTitle as string,
      icon: 'bi bi-globe',
      id: this.id,
      children: languages,
    }))
  );

  constructor(@Inject(LPX_LANGUAGE) private languages: LpxLanguage[]) {
    this.init(this.languages);
  }

  setLanguages(languages: LpxLanguage[]) {
    this.init(languages);
  }

  init(languages: LpxLanguage[]): void {
    this.store.patch({
      languages,
      selectedLanguage: languages.find((lang) => lang.selected),
    });
  }

  setSelectedLanguage(lang: LpxLanguage) {
    this.store.patch({
      selectedLanguage: lang,
    });
  }
}
