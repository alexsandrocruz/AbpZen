import { ToolbarAction } from '@abp/ng.components/extensible';
import { LanguageTextDto } from '@volo/abp.ng.language-management/proxy';

export const DEFAULT_LANGUAGE_TEXTS_TOOLBAR_ACTIONS = ToolbarAction.createMany<LanguageTextDto[]>(
  [],
);
