import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { TemplateDefinitionDto } from '@volo/abp.ng.text-template-management/proxy';
import { of } from 'rxjs';

export const DEFAULT_TEXT_TEMPLATES_ENTITY_PROPS = EntityProp.createMany<TemplateDefinitionDto>([
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'TextTemplateManagement::Name',
    columnWidth: 300,
  },
  {
    type: ePropType.Boolean,
    name: 'isInlineLocalized',
    displayName: 'TextTemplateManagement::IsInlineLocalized',
    columnWidth: 150,
  },
  {
    type: ePropType.String,
    name: 'isLayout',
    displayName: 'TextTemplateManagement::IsLayout',
    columnWidth: 150,
    valueResolver: data => {
      const icon = data.record.isLayout
        ? '<div class="text-success"><i class="fa fa-check"></i></div>'
        : '';
      return of(icon);
    },
  },
  {
    type: ePropType.String,
    name: 'layout',
    displayName: 'TextTemplateManagement::Layout',
    columnWidth: 300,
  },
  {
    type: ePropType.String,
    name: 'defaultCultureName',
    displayName: 'TextTemplateManagement::DefaultCultureName',
    columnWidth: 200,
  },
]);
