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
import {
  FileManagementEntityActionContributors,
  FileManagementToolbarActionContributors,
  FileManagementEntityPropContributors,
} from '../models';
import {
  FILE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  FILE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
  FILE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  DEFAULT_FILE_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_FILE_MANAGEMENT_TOOLBAR_ACTIONS,
  DEFAULT_FILE_MANAGEMENT_ENTITY_PROPS,
} from '../tokens';

export const fileManagementExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: FileManagementEntityActionContributors =
    injector.get(FILE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: FileManagementToolbarActionContributors =
    injector.get(FILE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: FileManagementEntityPropContributors =
    injector.get(FILE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'FileManagement').pipe(
    map(entities => ({
      /**
       * TODO: uncomment following statement with correct entity name
       * Following statement is commented out on 02 Sept 2020
       * It
       */
      // [eFileManagementComponents.FolderContent]: entities.DirectoryContentDto,
    })),
    mapEntitiesToContributors(injector, 'FileManagement'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_FILE_MANAGEMENT_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_FILE_MANAGEMENT_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_FILE_MANAGEMENT_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
    }),
  );
};
