import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { inject, Injectable, Injector } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import {
  FILE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  FILE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  DEFAULT_FILE_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_FILE_MANAGEMENT_ENTITY_PROPS,
  DEFAULT_FILE_MANAGEMENT_TOOLBAR_ACTIONS,
  FILE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import {
  FileManagementEntityPropContributors,
  FileManagementEntityActionContributors,
  FileManagementToolbarActionContributors,
} from './models/config-options';

/**
 * @deprecated Use `fileManagementExtensionsResolver` *function* instead.
 */
@Injectable()
export class FileManagementInitializer {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: FileManagementEntityActionContributors =
      this.injector.get(FILE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: FileManagementToolbarActionContributors =
      this.injector.get(FILE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: FileManagementEntityPropContributors =
      this.injector.get(FILE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'FileManagement').pipe(
      map(entities => ({
        /**
         * TODO: uncomment following statement with correct entity name
         * Following statement is commented out on 02 Sept 2020
         * It
         */
        // [eFileManagementComponents.FolderContent]: entities.DirectoryContentDto,
      })),
      mapEntitiesToContributors(this.injector, 'FileManagement'),
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
      map(() => true),
    );
  }
}
