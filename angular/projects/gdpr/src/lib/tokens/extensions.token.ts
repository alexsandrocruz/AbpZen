import { DEFAULT_PERSONAL_DATA_ENTITY_PROPS } from '../defaults/default-gdpr-entity-props';
import { eGdprComponents } from '../enums/components';
import { DEFAULT_GDPR_TOOLBAR } from '../defaults/default-gdpr-toolbar-actions';

export const DEFAULT_GDPR_ENTITY_PROPS = {
  [eGdprComponents.PersonalData]: DEFAULT_PERSONAL_DATA_ENTITY_PROPS,
};

export const DEFAULT_GDPR_TOOLBAR_ACTIONS = {
  [eGdprComponents.PersonalData]: DEFAULT_GDPR_TOOLBAR,
};
