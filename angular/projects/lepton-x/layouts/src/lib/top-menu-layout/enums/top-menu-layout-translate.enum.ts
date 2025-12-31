export enum TopMenuLayoutTranslate {
  GeneralSettings = 'topMenuLayout.settings.generalSettings',
}

export type TopMenuLayoutTranslateValues = {
  [key in TopMenuLayoutTranslate]: string;
};

export const TopMenuLayoutTranslateDefaults: TopMenuLayoutTranslateValues = {
  [TopMenuLayoutTranslate.GeneralSettings]: 'General Settings',
};
