import { LpxLocalStorageService } from '@volo/ngx-lepton-x.core';
import { Inject, Injectable, Injector } from '@angular/core';
import {
  DataStore,
  LanguageService,
  LpxNavbarItem,
  LpxStyle,
  StyleService,
  WINDOW,
} from '@volo/ngx-lepton-x.core';
import { fromEvent } from 'rxjs';
import {
  distinctUntilKeyChanged,
  filter,
  map,
  startWith,
  takeWhile,
} from 'rxjs/operators';
import { StyleNames, ThemeTranslateKeys } from './enums';
import {
  LpxTheme,
  LpxThemeOptions,
  LpxThemes,
  LpxThemeStyleConfig,
  SystemKey,
} from './models';
import { LPX_THEME_OPTIONS, LPX_THEMES } from './tokens';

@Injectable()
export class ThemeService {
  readonly store = new DataStore({
    styles: [] as LpxTheme[],
    selectedStyle: {} as LpxTheme,
  });

  id = 'styles';

  convertThemeToNavbarItem = (styles: LpxTheme[]): LpxNavbarItem[] => {
    return styles.map((style) => ({
      icon: style.icon,
      text: style.label,
      order: style.order,
      id: style.styleName,
      selected: style.styleName === this.selectedStyle.styleName,
      action: () => {
        if (this.selectedStyle.styleName === style.styleName) {
          return true;
        }
        return this.setTheme(style);
      },
    }));
  };

  get selectedStyle(): LpxThemeStyleConfig {
    return this.store.state.selectedStyle;
  }

  selectedStyle$ = this.store.sliceState((state) => state.selectedStyle);
  styles$ = this.store.sliceState((state) => state.styles);
  stylesAsNavbarItems$ = this.styles$.pipe(map(this.convertThemeToNavbarItem));
  // TODO: PROVIDE API
  stylesAsSettingsGroup$ = this.stylesAsNavbarItems$.pipe(
    map((styles) => ({
      text: ThemeTranslateKeys.AppearanceTitle,
      icon: 'bi bi-palette-fill',
      id: this.id,
      children: styles,
    }))
  );

  systemMediaQueryList = this.window.matchMedia('(prefers-color-scheme: dark)');
  systemThemeChange$ = fromEvent<MediaQueryList>(
    this.systemMediaQueryList,
    'change'
  ).pipe(
    startWith(this.systemMediaQueryList),
    map((list) => list.matches),
    filter(() => this.selectedStyle.styleName === StyleNames.System),
    takeWhile(() => !this.themeSettings.disableSystemOption)
  );

  get currentSystemTheme() {
    return this.systemMediaQueryList.matches ? 'dark' : 'light';
  }

  constructor(
    @Inject(LPX_THEME_OPTIONS) public themeSettings: LpxThemeOptions,
    @Inject(LPX_THEMES) public styles: LpxThemes,
    private injector: Injector,
    @Inject(WINDOW) protected window: any,
    private languageService: LanguageService,
    private styleService: StyleService,
    private localStorageService:LpxLocalStorageService
  ) {
    if (!this.themeSettings.disableSystemOption) {
      this.createSystemOption();
    }
    this.subscribeDirectionChange();
  }

  getInitialTheme() {
    const defaultStyle = this.styles.find(({ defaultTheme }) => defaultTheme);
    const savedStyle = this.styles.find(
      ({ styleName }) =>
        styleName === this.localStorageService.getItem(this.themeSettings.localStorageKey)
    );
    if (this.themeSettings.disableSystemOption) {
      return savedStyle || defaultStyle;
    } else {
      const system = this.findLpxStyleByName(StyleNames.System);
      return savedStyle || defaultStyle || system;
    }
  }

  createSystemOption() {
    const systemOption = new LpxTheme({
      styleName: StyleNames.System,
      label: ThemeTranslateKeys.System,
      icon: 'bi bi-laptop-fill',
      bundles: [],
    });
    this.styles.push(systemOption);
    this.systemThemeChange$.subscribe(() => {
      this.setTheme(systemOption);
    });
  }

  async initTheme(): Promise<void> {
    const style = this.getInitialTheme();
    if (style) {
      await this.setTheme(style);
      this.store.patch({ styles: this.styles });
    }
    return Promise.resolve();
  }

  async setTheme(style?: LpxTheme): Promise<boolean> {
    if (!style) {
      return Promise.resolve(true);
    }
    const styleService = this.injector.get(StyleService);
    const { styleName, bundles, systemTheme } = this.getBundlesAndStyle(style);

    for (const bundle of bundles) {
      await styleService.loadStyle(
        bundle,
        this.languageService.selectedLanguage?.isRTL ? 'rtl' : 'ltr'
      );
    }
    this.activateStyle(styleName, systemTheme);

    this.localStorageService.setItem(this.themeSettings.localStorageKey, style.styleName);
    this.store.patch({ selectedStyle: style });

    return Promise.resolve(true);
  }

  private getBundlesAndStyle(style: LpxTheme) {
    let styleName = style.styleName;
    let bundles = style.bundles;
    let systemTheme: SystemKey = this.currentSystemTheme;

    if (style.styleName === StyleNames.System) {
      const activeStyle = this.styles.find(
        (s) => s.systemKey === this.currentSystemTheme
      );

      if (activeStyle) {
        styleName = activeStyle.styleName;
        bundles = activeStyle.bundles;
      } else {
        throw new Error(
          `System style ${this.currentSystemTheme} not found.
          Please add systemKey with value ${this.currentSystemTheme} to the style`
        );
      }
      /*
       * The reason for this opposite condition is when the user changes the system theme old theme is the opposite of the current system theme
       * */
      systemTheme = this.currentSystemTheme === 'dark' ? 'light' : 'dark';
    }
    return { styleName, bundles, systemTheme };
  }

  private activateStyle(styleName: string, systemTheme: SystemKey) {
    if (this.selectedStyle.styleName === StyleNames.System) {
      const selectedStyle = this.styles.find(
        (s) => s.systemKey === systemTheme
      );
      if (selectedStyle) {
        this.removeOldThemeStyles(selectedStyle.bundles);
        this.setThemeClassToBody(styleName, selectedStyle.styleName);
      }
    } else {
      if (this.selectedStyle.styleName) {
        this.removeOldThemeStyles(this.selectedStyle.bundles);
      }
      this.setThemeClassToBody(styleName, this.selectedStyle.styleName);
    }
  }

  private findLpxStyleByName(styleName: string) {
    return this.styles.find((style) => style.styleName === styleName);
  }

  private setThemeClassToBody(styleName: string, selectedStyleName?: string) {
    const body = document.querySelector('body');
    if (body) {
      if (selectedStyleName) {
        body.classList.remove(`lpx-theme-${selectedStyleName}`);
      }
      body.classList.add(`lpx-theme-${styleName}`);
    }
  }

  private removeOldThemeStyles(bundles: LpxStyle[]) {
    bundles.forEach((bundle) => {
      const elem = document.querySelector(`link#${bundle.bundleName}`);
      elem?.remove();
    });
  }

  private subscribeDirectionChange() {
    this.languageService.languageChange$
      .pipe(distinctUntilKeyChanged('isRTL'))
      .subscribe(async (lang) => {
        const bundles = this.selectedStyle.bundles || [];
        if (
          !bundles.length &&
          this.selectedStyle.styleName === StyleNames.System
        ) {
          const selectedStyle = this.styles.find(
            (s) => s.systemKey === this.currentSystemTheme
          );
          if (selectedStyle) {
            bundles.push(...selectedStyle.bundles);
          }
        }
        const direction = lang?.isRTL ? 'rtl' : 'ltr';
        for (const bundle of this.selectedStyle.bundles || []) {
          await this.styleService.replaceStyle(bundle, direction);
        }
      });
  }
}
