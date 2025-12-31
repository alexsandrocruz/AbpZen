import { Inject, Injectable, Optional } from '@angular/core';
import { map } from 'rxjs/operators';
import { combineLatest, Observable } from 'rxjs';
import { ThemeService } from '@volosoft/ngx-lepton-x';
import { LanguageService } from '@volo/ngx-lepton-x.core';
import { SideMenuLayoutService } from '../side-menu-layout.service';
import { combineAndFilterByChildren } from '../../utils/stream-utils';
import { Setting } from '../../components/settings/setting.model';
import { SettingsService } from '../../components/settings/settings-service.model';
 import { DefaultSelectedLanguageLabelFn } from '../defaults';
import { LpxSelectedLanguageLabelFnType } from '../../models';
import { LPX_SELECTED_LANGUAGE_LABEL_FN } from '../../tokens';

@Injectable({
  providedIn: 'root',
})
export class SidebarSettingsService implements SettingsService {
  settings$: Observable<Setting[]> = combineAndFilterByChildren([
    this.themeService.stylesAsSettingsGroup$,
    this.layoutService.layoutsAsSettingsGroup$,
    this.languageService.languagesAsSettingsGroup$,
  ]);

  selectedSettings$ = combineLatest([
    this.themeService.selectedStyle$.pipe(
      map((style) => ({
        icon: style.icon,
        id: this.themeService.id,
      }))
    ),
    this.layoutService.selectedLayout$.pipe(
      map((layout) => ({
        icon: layout.icon,
        id: this.layoutService.id,
      }))
    ),
    this.languageService.selectedLanguage$.pipe(
      map((lang) =>  this._selectedLanguageLabelFn(lang, this.languageService.id))
    ),
  ]);

  private _selectedLanguageLabelFn: LpxSelectedLanguageLabelFnType;
  constructor(
    private themeService: ThemeService,
    private languageService: LanguageService,
    private layoutService: SideMenuLayoutService,
    @Inject(LPX_SELECTED_LANGUAGE_LABEL_FN) 
    @Optional()
    selectedLanguageLabelFn: LpxSelectedLanguageLabelFnType | undefined ,  
  ) {
    this._selectedLanguageLabelFn =  selectedLanguageLabelFn || DefaultSelectedLanguageLabelFn
  }
}
