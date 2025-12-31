import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
  mergeWithDefaultActions,
} from '@abp/ng.components/extensible';
import { Injector, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap } from 'rxjs';
import { eGdprComponents } from '../enums';
import { GdprEntityPropContributors, GdprToolbarActionContributors } from '../models';
import {
  DEFAULT_GDPR_ENTITY_PROPS,
  DEFAULT_GDPR_TOOLBAR_ACTIONS,
} from '../tokens/extensions.token';

export const gdprExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const propContributors: GdprEntityPropContributors =
    injector.get(DEFAULT_GDPR_ENTITY_PROPS, null) || {};
  const toolbarContributors: GdprToolbarActionContributors =
    injector.get(DEFAULT_GDPR_TOOLBAR_ACTIONS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'Gdpr').pipe(
    map(entities => ({
      [eGdprComponents.PersonalData]: entities.PersonalData,
    })),
    mapEntitiesToContributors(injector, 'AbpGdpr'),
    tap(objectExtensionContributors => {
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_GDPR_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_GDPR_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
    }),
  );
};
