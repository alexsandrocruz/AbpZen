import { EntityAction } from '@abp/ng.components/extensible';
import { LanguageTextDto } from '@volo/abp.ng.language-management/proxy';
import { LanguageTextsComponent } from '../components/language-texts/language-texts.component';

export const DEFAULT_LANGUAGE_TEXTS_ENTITY_ACTIONS = EntityAction.createMany<LanguageTextDto>([
  {
    text: 'LanguageManagement::Edit',
    action: data => {
      const component = data.getInjected(LanguageTextsComponent);
      component.edit(data.record, data.index);
    },
    permission: 'LanguageManagement.LanguageTexts.Edit',
  },
]);
