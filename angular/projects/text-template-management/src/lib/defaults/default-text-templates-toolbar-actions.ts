import { ToolbarAction } from '@abp/ng.components/extensible';
import { TemplateDefinitionDto } from '@volo/abp.ng.text-template-management/proxy';

export const DEFAULT_TEXT_TEMPLATES_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  TemplateDefinitionDto[]
>([]);
