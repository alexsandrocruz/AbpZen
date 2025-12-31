import {
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { eGdprComponents } from '../enums/components';
import { GdprRequestDto } from '@volo/abp.ng.gdpr/proxy';

export type GdprEntityPropContributors = Partial<{
  [eGdprComponents.PersonalData]: EntityPropContributorCallback<GdprRequestDto>[];
}>;

export type GdprToolbarActionContributors = Partial<{
  [eGdprComponents.PersonalData]: ToolbarActionContributorCallback<GdprRequestDto[]>[];
}>;

export interface GdprConfigOptions {
  createFormPropContributors?: GdprEntityPropContributors;
  toolbarActionContributors?: GdprToolbarActionContributors;
}
