import { LpxLanguage } from '@volo/ngx-lepton-x.core';
import { LpxContainer, LpxSelectedLanguageLabelFnType } from '../models';
import { SideMenuLayoutTranslate } from './enums';
// TODO: PROVIDE API
export const SideMenuContainerDefaults: LpxContainer[] = [
  {
    icon: 'bi bi-square',
    label: SideMenuLayoutTranslate.BoxedContainer,
    layoutClass: 'boxed',
    order: 0,
  },
  {
    icon: 'bi bi-layout-three-columns',
    label: SideMenuLayoutTranslate.FullWidthContainer,
    layoutClass: 'full',
    order: 1,
    default: true,
  },

  {
    icon: 'bi bi-code-square',
    label: SideMenuLayoutTranslate.FluidContainer,
    layoutClass: 'fixed',
    order: 2,
  },
];

export const DefaultSelectedLanguageLabelFn: LpxSelectedLanguageLabelFnType = (
  lang: LpxLanguage | undefined,
  id: string | undefined
) => {
  return {
    shortText: lang?.cultureName?.toLocaleUpperCase(),
    id: id,
  };
};
