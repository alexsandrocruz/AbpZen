import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { Injector, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';

import { map, tap } from 'rxjs/operators';
import { eTextTemplateManagementComponents } from '../enums/components';
import {
  TextTemplateManagementEntityActionContributors,
  TextTemplateManagementEntityPropContributors,
  TextTemplateManagementToolbarActionContributors,
} from '../models/config-options';
import {
  DEFAULT_TEXT_TEMPLATE_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_TEXT_TEMPLATE_MANAGEMENT_ENTITY_PROPS,
  DEFAULT_TEXT_TEMPLATE_MANAGEMENT_TOOLBAR_ACTIONS,
  TEXT_TEMPLATE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TEXT_TEMPLATE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TEXT_TEMPLATE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

export const textTemplateManagementExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);
  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: TextTemplateManagementEntityActionContributors =
    injector.get(TEXT_TEMPLATE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: TextTemplateManagementToolbarActionContributors =
    injector.get(TEXT_TEMPLATE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: TextTemplateManagementEntityPropContributors =
    injector.get(TEXT_TEMPLATE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'TextTemplateManagement').pipe(
    map(entities => ({
      [eTextTemplateManagementComponents.TextTemplates]: entities.TextDefinition,
    })),
    mapEntitiesToContributors(injector, 'TextTemplateManagement'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_TEXT_TEMPLATE_MANAGEMENT_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_TEXT_TEMPLATE_MANAGEMENT_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_TEXT_TEMPLATE_MANAGEMENT_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
    }),
  );
};
