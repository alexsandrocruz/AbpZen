import { Validators } from '@angular/forms';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { LocalizationService } from '@abp/ng.core';
import { ePropType, FormProp, PropData } from '@abp/ng.components/extensible';
import { LanguageDto } from '@volo/abp.ng.language-management/proxy';
import { LanguagesComponent } from '../components/languages/languages.component';

function EMPTY_OPTION<T>(data: Omit<PropData<T>, 'data'>) {
  const localizationService = data.getInjected(LocalizationService);

  return {
    value: null,
    key: localizationService.instant('AbpUi::NotAssigned'),
  };
}

export const DEFAULT_LANGUAGES_CREATE_FORM_PROPS = FormProp.createMany<LanguageDto>([
  {
    type: ePropType.String,
    name: 'cultureName',
    displayName: 'LanguageManagement::CultureName',
    id: 'culture-name',
    validators: () => [Validators.required],
    options: data =>
      data.getInjected(LanguagesComponent).cultures$.pipe(
        map(cultures => {
          return [
            EMPTY_OPTION(data),
            ...cultures.map(culture => ({
              key: culture.displayName,
              value: culture.name,
            })),
          ];
        }),
      ),
  },
  {
    type: ePropType.String,
    name: 'uiCultureName',
    displayName: 'LanguageManagement::UiCultureName',
    id: 'ui-culture-name',
    validators: () => [Validators.required],
    options: data =>
      data.getInjected(LanguagesComponent).cultures$.pipe(
        map(cultures => {
          return [
            EMPTY_OPTION(data),
            ...cultures.map(culture => ({
              key: culture.displayName,
              value: culture.name,
            })),
          ];
        }),
      ),
  },
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'LanguageManagement::DisplayName',
    id: 'name',
    validators: () => [Validators.maxLength(256)],
  },
  {
    type: ePropType.String,
    name: 'flagIcon',
    displayName: 'LanguageManagement::FlagIcon',
    id: 'flag-icon',
    validators: () => [Validators.required],
    options: data =>
      of(
        data.getInjected(LanguagesComponent).flagIcons.map(flag => ({
          key: flag,
          value: flag,
        })),
      ),
  },

  {
    type: ePropType.Boolean,
    name: 'isEnabled',
    displayName: 'LanguageManagement::IsEnabled',
    id: 'is-enabled',
    defaultValue: false,
  },
]);

export const DEFAULT_LANGUAGES_EDIT_FORM_PROPS = DEFAULT_LANGUAGES_CREATE_FORM_PROPS.slice(2);
