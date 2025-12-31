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
import { eOpenIddictProComponents } from '../enums/components';
import {
  OpenIddictProCreateFormPropContributors,
  OpenIddictProEditFormPropContributors,
  OpenIddictProEntityActionContributors,
  OpenIddictProEntityPropContributors,
  OpenIddictProToolbarActionContributors,
} from '../models/config-options';
import {
  DEFAULT_OPENIDDICT_PRO_CREATE_FORM_PROPS,
  DEFAULT_OPENIDDICT_PRO_EDIT_FORM_PROPS,
  DEFAULT_OPENIDDICT_PRO_ENTITY_ACTIONS,
  DEFAULT_OPENIDDICT_PRO_ENTITY_PROPS,
  DEFAULT_OPENIDDICT_PRO_TOOLBAR_ACTIONS,
  OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS,
  OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

/**
 * @deprecated Use `openIddictProExtensionsResolver` *function* instead.
 */
@Injectable()
export class OpenIddictProExtensionsGuard {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: OpenIddictProEntityActionContributors =
      this.injector.get(OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: OpenIddictProToolbarActionContributors =
      this.injector.get(OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: OpenIddictProEntityPropContributors =
      this.injector.get(OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: OpenIddictProCreateFormPropContributors =
      this.injector.get(OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: OpenIddictProEditFormPropContributors =
      this.injector.get(OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'OpenIddictPro').pipe(
      map(entities => {
        return {
          [eOpenIddictProComponents.Applications]: entities.ApiResource,
          [eOpenIddictProComponents.Scopes]: entities.Scopes,
        };
      }),
      mapEntitiesToContributors(this.injector, 'AbpOpenIddictPro'),
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
      mapTo(true),
    );
  }
}
