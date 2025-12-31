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
import { eOpenIddictProComponents } from '../enums';
import {
  OpenIddictProEntityActionContributors,
  OpenIddictProToolbarActionContributors,
  OpenIddictProEntityPropContributors,
  OpenIddictProCreateFormPropContributors,
  OpenIddictProEditFormPropContributors,
} from '../models';
import {
  OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS,
  OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS,
  OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_OPENIDDICT_PRO_ENTITY_ACTIONS,
  DEFAULT_OPENIDDICT_PRO_TOOLBAR_ACTIONS,
  DEFAULT_OPENIDDICT_PRO_ENTITY_PROPS,
  DEFAULT_OPENIDDICT_PRO_CREATE_FORM_PROPS,
  DEFAULT_OPENIDDICT_PRO_EDIT_FORM_PROPS,
} from '../tokens';

export const openIddictProExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: OpenIddictProEntityActionContributors =
    injector.get(OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: OpenIddictProToolbarActionContributors =
    injector.get(OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: OpenIddictProEntityPropContributors =
    injector.get(OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS, null) || {};
  const createFormContributors: OpenIddictProCreateFormPropContributors =
    injector.get(OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
  const editFormContributors: OpenIddictProEditFormPropContributors =
    injector.get(OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'OpenIddictPro').pipe(
    map(entities => {
      return {
        [eOpenIddictProComponents.Applications]: entities.ApiResource,
        [eOpenIddictProComponents.Scopes]: entities.Scopes,
      };
    }),
    mapEntitiesToContributors(injector, 'AbpOpenIddictPro'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_OPENIDDICT_PRO_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_OPENIDDICT_PRO_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_OPENIDDICT_PRO_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
      mergeWithDefaultProps(
        extensions.createFormProps,
        DEFAULT_OPENIDDICT_PRO_CREATE_FORM_PROPS,
        objectExtensionContributors.createForm,
        createFormContributors,
      );
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_OPENIDDICT_PRO_EDIT_FORM_PROPS,
        objectExtensionContributors.editForm,
        editFormContributors,
      );
    }),
  );
};
