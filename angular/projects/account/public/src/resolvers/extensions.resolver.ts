import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
  mergeWithDefaultActions,
} from '@abp/ng.components/extensible';
import { inject, Injector } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap, zip } from 'rxjs';
import { eAccountComponents } from '../enums';
import {
  ACCOUNT_ENTITY_ACTION_CONTRIBUTORS,
  ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS,
  ACCOUNT_ENTITY_PROP_CONTRIBUTORS,
  ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_ACCOUNT_FORM_PROPS,
  DEFAULT_ACCOUNT_ENTITY_ACTIONS,
  DEFAULT_ACCOUNT_TOOLBAR_ACTIONS,
  DEFAULT_ACCOUNT_ENTITY_PROPS,
} from '../tokens/extensions.token';
import {
  AccountEditFormPropContributors,
  AccountEntityActionContributors,
  AccountEntityPropContributors,
  AccountToolbarActionContributors,
} from '../models/config-options';

export const accountExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: AccountEntityActionContributors =
    injector.get(ACCOUNT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: AccountToolbarActionContributors =
    injector.get(ACCOUNT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: AccountEntityPropContributors =
    injector.get(ACCOUNT_ENTITY_PROP_CONTRIBUTORS, null) || {};
  const formContributors: AccountEditFormPropContributors =
    injector.get(ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

  const profileSettingsObserve = getObjectExtensionEntitiesFromStore(injector, 'Identity').pipe(
    map(entities => {
      return {
        [eAccountComponents.PersonalSettings]: entities.User,
      };
    }),
    mapEntitiesToContributors(injector, 'AbpIdentity'),
    tap(objectExtensionContributors => {
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_ACCOUNT_FORM_PROPS,
        objectExtensionContributors.editForm,
        formContributors,
      );
    }),
  );

  const accountObserve = getObjectExtensionEntitiesFromStore(injector, 'Account').pipe(
    map(entities => ({
      [eAccountComponents.MySecurityLogs]: entities.SecurityLogs,
    })),
    mapEntitiesToContributors(injector, 'AbpAccount'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_ACCOUNT_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_ACCOUNT_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_ACCOUNT_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
    }),
  );

  return zip(accountObserve, profileSettingsObserve);
};
