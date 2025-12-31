import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { InjectionToken } from '@angular/core';
import { DEFAULT_APPLICATIONS_ENTITY_ACTIONS } from '../defaults/default-applications-entity-actions';
import { DEFAULT_APPLICATIONS_ENTITY_PROPS } from '../defaults/default-applications-entity-props';
import { DEFAULT_APPLICATIONS_TOOLBAR_ACTIONS } from '../defaults/default-applications-toolbar-actions';
import {
  DEFAULT_APPLICATIONS_CREATE_FORM_PROPS,
  DEFAULT_APPLICATIONS_FORM_PROPS,
} from '../defaults/default-applications-form-props';
import { eOpenIddictProComponents } from '../enums';
import { Applications, Scopes } from '@volo/abp.ng.openiddictpro/proxy';
import { DEFAULT_SCOPES_ENTITY_PROPS } from '../defaults/default-scope-entity-props';
import { DEFAULT_SCOPES_TOOLBAR_ACTIONS } from '../defaults/default-scope-toolbar-actions';
import {
  DEFAULT_SCOPE_CREATE_FORM_PROPS,
  DEFAULT_SCOPES_FORM_PROPS,
} from '../defaults/default-scope-form-props';
import { DEFAULT_SCOPE_ENTITY_ACTIONS } from '../defaults/default-scope-entity-actions';

export const DEFAULT_OPENIDDICT_PRO_ENTITY_ACTIONS = {
  [eOpenIddictProComponents.Applications]: DEFAULT_APPLICATIONS_ENTITY_ACTIONS,
  [eOpenIddictProComponents.Scopes]: DEFAULT_SCOPE_ENTITY_ACTIONS,
};

export const DEFAULT_OPENIDDICT_PRO_TOOLBAR_ACTIONS = {
  [eOpenIddictProComponents.Applications]: DEFAULT_APPLICATIONS_TOOLBAR_ACTIONS,
  [eOpenIddictProComponents.Scopes]: DEFAULT_SCOPES_TOOLBAR_ACTIONS,
};

export const DEFAULT_OPENIDDICT_PRO_ENTITY_PROPS = {
  [eOpenIddictProComponents.Applications]: DEFAULT_APPLICATIONS_ENTITY_PROPS,
  [eOpenIddictProComponents.Scopes]: DEFAULT_SCOPES_ENTITY_PROPS,
};

export const DEFAULT_OPENIDDICT_PRO_CREATE_FORM_PROPS = {
  [eOpenIddictProComponents.Applications]: DEFAULT_APPLICATIONS_CREATE_FORM_PROPS,
  [eOpenIddictProComponents.Scopes]: DEFAULT_SCOPE_CREATE_FORM_PROPS,
};

export const DEFAULT_OPENIDDICT_PRO_EDIT_FORM_PROPS = {
  [eOpenIddictProComponents.Applications]: DEFAULT_APPLICATIONS_FORM_PROPS,
  [eOpenIddictProComponents.Scopes]: DEFAULT_SCOPES_FORM_PROPS,
};

export const OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS =
  new InjectionToken<EntityActionContributors>('OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS');

export const OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS =
  new InjectionToken<ToolbarActionContributors>('OPENIDDICT_PRO__TOOLBAR_ACTION_CONTRIBUTORS');

export const OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
  'OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS',
);

export const OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<CreateFormPropContributors>('OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS');

export const OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<EditFormPropContributors>('OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS');

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EntityActionContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;
type ToolbarActionContributors = Partial<{
  [eOpenIddictProComponents.Applications]: ToolbarActionContributorCallback<
    Applications.Dtos.ApplicationDto[]
  >[];
}>;
type EntityPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EntityPropContributorCallback<Applications.Dtos.ApplicationDto>[];
  [eOpenIddictProComponents.Scopes]: EntityPropContributorCallback<Scopes.Dtos.ScopeDto>[];
}>;
type CreateFormPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: CreateFormPropContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;
type EditFormPropContributors = Partial<{
  [eOpenIddictProComponents.Applications]: EditFormPropContributorCallback<Applications.Dtos.ApplicationDto>[];
}>;
