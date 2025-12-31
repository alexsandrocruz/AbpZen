import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';

import { inject, Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map, mapTo, tap } from 'rxjs/operators';

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

/**
 * @deprecated Use `textTemplateManagementExtensionsResolver` *function* instead.
 */
@Injectable()
export class TextTemplateManagementExtensionsGuard {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: TextTemplateManagementEntityActionContributors =
      this.injector.get(TEXT_TEMPLATE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: TextTemplateManagementToolbarActionContributors =
      this.injector.get(TEXT_TEMPLATE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: TextTemplateManagementEntityPropContributors =
      this.injector.get(TEXT_TEMPLATE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'TextTemplateManagement').pipe(
      map(entities => ({
        [eTextTemplateManagementComponents.TextTemplates]: entities.TextDefinition,
      })),
      mapEntitiesToContributors(this.injector, 'TextTemplateManagement'),
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
      mapTo(true),
    );
  }
}
