import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { Injector, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap } from 'rxjs';
import { eSaasComponents } from '../enums';
import {
  SaasEntityActionContributors,
  SaasToolbarActionContributors,
  SaasEntityPropContributors,
  SaasCreateFormPropContributors,
  SaasEditFormPropContributors,
} from '../models';
import {
  SAAS_ENTITY_ACTION_CONTRIBUTORS,
  SAAS_TOOLBAR_ACTION_CONTRIBUTORS,
  SAAS_ENTITY_PROP_CONTRIBUTORS,
  SAAS_CREATE_FORM_PROP_CONTRIBUTORS,
  SAAS_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_SAAS_ENTITY_ACTIONS,
  DEFAULT_SAAS_TOOLBAR_ACTIONS,
  DEFAULT_SAAS_ENTITY_PROPS,
  DEFAULT_SAAS_CREATE_FORM_PROPS,
  DEFAULT_SAAS_EDIT_FORM_PROPS,
} from '../tokens';

export const saasExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: SaasEntityActionContributors =
    injector.get(SAAS_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: SaasToolbarActionContributors =
    injector.get(SAAS_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: SaasEntityPropContributors =
    injector.get(SAAS_ENTITY_PROP_CONTRIBUTORS, null) || {};
  const createFormContributors: SaasCreateFormPropContributors =
    injector.get(SAAS_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
  const editFormContributors: SaasEditFormPropContributors =
    injector.get(SAAS_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'Saas').pipe(
    map(entities => ({
      [eSaasComponents.Editions]: entities.Edition,
      [eSaasComponents.Tenants]: entities.Tenant,
    })),
    mapEntitiesToContributors(injector, 'Saas'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_SAAS_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_SAAS_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_SAAS_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
      mergeWithDefaultProps(
        extensions.createFormProps,
        DEFAULT_SAAS_CREATE_FORM_PROPS,
        objectExtensionContributors.createForm,
        createFormContributors,
      );
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_SAAS_EDIT_FORM_PROPS,
        objectExtensionContributors.editForm,
        editFormContributors,
      );
    }),
  );
};
