import { eOpenIddictProComponents } from '../enums';

import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { Applications, Scopes } from '@volo/abp.ng.openiddictpro/proxy';

export type OpenIddictProEntityActionContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EntityActionContributorCallback<Applications.Dtos.ApplicationDto>[];
  [eOpenIddictProComponents.Scopes]: EntityActionContributorCallback<Scopes.Dtos.ScopeDto>[];
}>;

export type OpenIddictProToolbarActionContributors = Partial<{
  [eOpenIddictProComponents.Applications]: ToolbarActionContributorCallback<
    Applications.Dtos.ApplicationDto[]
  >[];
}>;

export type OpenIddictProEntityPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EntityPropContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;

export type OpenIddictProCreateFormPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: CreateFormPropContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;

export type OpenIddictProEditFormPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EditFormPropContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;

export interface OpenIddictProConfigOptions {
  entityActionContributors?: OpenIddictProEntityActionContributors;
  toolbarActionContributors?: OpenIddictProToolbarActionContributors;
  entityPropContributors?: OpenIddictProEntityPropContributors;
  createFormPropContributors?: OpenIddictProCreateFormPropContributors;
  editFormPropContributors?: OpenIddictProEditFormPropContributors;
}
