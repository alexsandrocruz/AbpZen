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
import { eSaasComponents } from '../enums/components';
import {
  SaasCreateFormPropContributors,
  SaasEditFormPropContributors,
  SaasEntityActionContributors,
  SaasEntityPropContributors,
  SaasToolbarActionContributors,
} from '../models/config-options';
import {
  DEFAULT_SAAS_CREATE_FORM_PROPS,
  DEFAULT_SAAS_EDIT_FORM_PROPS,
  DEFAULT_SAAS_ENTITY_ACTIONS,
  DEFAULT_SAAS_ENTITY_PROPS,
  DEFAULT_SAAS_TOOLBAR_ACTIONS,
  SAAS_CREATE_FORM_PROP_CONTRIBUTORS,
  SAAS_EDIT_FORM_PROP_CONTRIBUTORS,
  SAAS_ENTITY_ACTION_CONTRIBUTORS,
  SAAS_ENTITY_PROP_CONTRIBUTORS,
  SAAS_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

/**
 * @deprecated Use `saasExtensionsResolver` *function* instead.
 */
@Injectable()
export class SaasExtensionsGuard {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: SaasEntityActionContributors =
      this.injector.get(SAAS_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: SaasToolbarActionContributors =
      this.injector.get(SAAS_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: SaasEntityPropContributors =
      this.injector.get(SAAS_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: SaasCreateFormPropContributors =
      this.injector.get(SAAS_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: SaasEditFormPropContributors =
      this.injector.get(SAAS_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'Saas').pipe(
      map(entities => ({
        [eSaasComponents.Editions]: entities.Edition,
        [eSaasComponents.Tenants]: entities.Tenant,
      })),
      mapEntitiesToContributors(this.injector, 'Saas'),
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
      map(() => true),
    );
  }
}
