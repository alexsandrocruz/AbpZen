export interface LpxLanguage {
  cultureName?: string;
  displayName?: string;
  selected?: boolean;
  isRTL?: boolean;
  twoLetterISOLanguageName?:string;
}
export interface LpxLanguageState {
  languages: LpxLanguage[];
  selectedLanguage?: LpxLanguage;
}

export interface LpxLanguageModuleOptions {
  languages?: LpxLanguage[];
}
