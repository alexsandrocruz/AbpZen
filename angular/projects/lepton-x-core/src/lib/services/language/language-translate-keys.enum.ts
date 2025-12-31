export enum LanguageTranslateKeys {
  SettingsTitle = 'language.settings.title',
}

export type LanguageTranslateValues = {
  [key in LanguageTranslateKeys]: string;
};

export const LanguageTranslateDefaults: LanguageTranslateValues = {
  [LanguageTranslateKeys.SettingsTitle]: 'Language Options',
};
