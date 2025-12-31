import { EntityAction } from '@abp/ng.components/extensible';
import { TemplateDefinitionDto } from '@volo/abp.ng.text-template-management/proxy';
import { TextTemplatesComponent } from '../components/text-templates/text-templates.component';

export const DEFAULT_TEXT_TEMPLATES_ENTITY_ACTIONS = EntityAction.createMany<TemplateDefinitionDto>(
  [
    {
      text: 'TextTemplateManagement::EditContents',
      action: data => {
        const component = data.getInjected(TextTemplatesComponent);
        component.editContents(data.record);
      },
      permission: 'TextTemplateManagement.TextTemplates.EditContents',
    },
  ],
);
