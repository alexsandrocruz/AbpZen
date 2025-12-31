import {
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { TemplateDefinitionDto } from '@volo/abp.ng.text-template-management/proxy';
import { eTextTemplateManagementComponents } from '../enums/components';

export type TextTemplateManagementEntityActionContributors = Partial<{
  [eTextTemplateManagementComponents.TextTemplates]: EntityActionContributorCallback<TemplateDefinitionDto>[];
}>;

export type TextTemplateManagementToolbarActionContributors = Partial<{
  [eTextTemplateManagementComponents.TextTemplates]: ToolbarActionContributorCallback<
    TemplateDefinitionDto[]
  >[];
}>;

export type TextTemplateManagementEntityPropContributors = Partial<{
  [eTextTemplateManagementComponents.TextTemplates]: EntityPropContributorCallback<TemplateDefinitionDto>[];
}>;

export interface TextTemplateManagementConfigOptions {
  entityActionContributors?: TextTemplateManagementEntityActionContributors;
  toolbarActionContributors?: TextTemplateManagementToolbarActionContributors;
  entityPropContributors?: TextTemplateManagementEntityPropContributors;
}
