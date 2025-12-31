import {
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { InjectionToken } from '@angular/core';
import { IdentitySecurityLogDto } from '@volo/abp.commercial.ng.ui/config';
import { DEFAULT_MY_SECURITY_LOGS_ENTITY_ACTIONS } from '../defaults/default-my-security-logs-entity-actions';
import { DEFAULT_MY_SECURITY_LOGS_ENTITY_PROPS } from '../defaults/default-my-security-logs-entity-props';
import { DEFAULT_MY_SECURITY_LOGS_TOOLBAR_ACTIONS } from '../defaults/default-my-security-logs-toolbar-actions';
import { eAccountComponents } from '../enums/components';
import { ProfileDto } from '@volo/abp.ng.account/public/proxy';
import { DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS } from '../defaults/default-personal-settings-form-props';

export const DEFAULT_ACCOUNT_ENTITY_ACTIONS = {
  [eAccountComponents.MySecurityLogs]: DEFAULT_MY_SECURITY_LOGS_ENTITY_ACTIONS,
};

export const DEFAULT_ACCOUNT_TOOLBAR_ACTIONS = {
  [eAccountComponents.MySecurityLogs]: DEFAULT_MY_SECURITY_LOGS_TOOLBAR_ACTIONS,
};

export const DEFAULT_ACCOUNT_ENTITY_PROPS = {
  [eAccountComponents.MySecurityLogs]: DEFAULT_MY_SECURITY_LOGS_ENTITY_PROPS,
};

export const DEFAULT_ACCOUNT_FORM_PROPS = {
  [eAccountComponents.PersonalSettings]: DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS,
};

export const ACCOUNT_ENTITY_ACTION_CONTRIBUTORS = new InjectionToken<EntityActionContributors>(
  'ACCOUNT_ENTITY_ACTION_CONTRIBUTORS',
);

export const ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS = new InjectionToken<ToolbarActionContributors>(
  'ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS',
);

export const ACCOUNT_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
  'ACCOUNT_ENTITY_PROP_CONTRIBUTORS',
);

export const ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS = new InjectionToken<EditFormPropContributors>(
  'ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS',
);

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [eAccountComponents.MySecurityLogs]: EntityActionContributorCallback<IdentitySecurityLogDto>[];
}>;
type ToolbarActionContributors = Partial<{
  [eAccountComponents.MySecurityLogs]: ToolbarActionContributorCallback<IdentitySecurityLogDto[]>[];
}>;
type EntityPropContributors = Partial<{
  [eAccountComponents.MySecurityLogs]: EntityPropContributorCallback<IdentitySecurityLogDto>[];
}>;
type EditFormPropContributors = Partial<{
  [eAccountComponents.PersonalSettings]: EditFormPropContributorCallback<ProfileDto>[];
}>;
