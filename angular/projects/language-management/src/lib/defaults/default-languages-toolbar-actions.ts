import { ToolbarAction } from '@abp/ng.components/extensible';
import { LanguageDto } from '@volo/abp.ng.language-management/proxy';
import { LanguagesComponent } from '../components/languages/languages.component';

export const DEFAULT_LANGUAGES_TOOLBAR_ACTIONS = ToolbarAction.createMany<LanguageDto[]>([
  {
    text: 'LanguageManagement::CreateNewLanguage',
    action: data => {
      const component = data.getInjected(LanguagesComponent);
      component.add();
    },
    permission: 'LanguageManagement.Languages.Create',
    icon: 'fa fa-plus',
  },
]);
