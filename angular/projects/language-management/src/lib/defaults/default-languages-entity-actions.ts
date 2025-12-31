import { EntityAction } from '@abp/ng.components/extensible';
import { LanguageDto } from '@volo/abp.ng.language-management/proxy';
import { LanguagesComponent } from '../components/languages/languages.component';

export const DEFAULT_LANGUAGES_ENTITY_ACTIONS = EntityAction.createMany<LanguageDto>([
  {
    text: 'LanguageManagement::Edit',
    action: data => {
      const component = data.getInjected(LanguagesComponent);
      component.edit(data.record.id);
    },
    permission: 'LanguageManagement.Languages.Edit',
  },
  {
    text: 'LanguageManagement::Delete',
    action: data => {
      const component = data.getInjected(LanguagesComponent);
      component.delete(data.record.id, data.record.displayName, data.record.isDefaultLanguage);
    },
    permission: 'LanguageManagement.Languages.Delete',
  },
  {
    text: 'LanguageManagement::SetAsDefaultLanguage',
    action: data => {
      const component = data.getInjected(LanguagesComponent);
      component.setAsDefault(data.record.id);
    },
    permission: 'LanguageManagement.Languages.ChangeDefault',
  },
]);
