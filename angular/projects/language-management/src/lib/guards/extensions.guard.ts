import {
  EntityProp,
  ExtensionsService,
  FormProp,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { ENABLE_FLAG_ICON } from '@volo/abp.commercial.ng.ui/config';
import { LanguageDto } from '@volo/abp.ng.language-management/proxy';
import { inject, Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map, mapTo, tap } from 'rxjs/operators';
import { eLanguageManagementComponents } from '../enums/components';
import {
  LanguageManagementCreateFormPropContributors,
  LanguageManagementEditFormPropContributors,
  LanguageManagementEntityActionContributors,
  LanguageManagementEntityPropContributors,
  LanguageManagementToolbarActionContributors,
} from '../models/config-options';
import {
  DEFAULT_LANGUAGE_MANAGEMENT_CREATE_FORM_PROPS,
  DEFAULT_LANGUAGE_MANAGEMENT_EDIT_FORM_PROPS,
  DEFAULT_LANGUAGE_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_LANGUAGE_MANAGEMENT_ENTITY_PROPS,
  DEFAULT_LANGUAGE_MANAGEMENT_TOOLBAR_ACTIONS,
  LANGUAGE_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

/**
 * @deprecated Use `languageManagementExtensionsResolver` *function* instead.
 */
@Injectable()
export class LanguageManagementExtensionsGuard {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const isFlagIconEnabled = this.injector.get(ENABLE_FLAG_ICON);
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: LanguageManagementEntityActionContributors =
      this.injector.get(LANGUAGE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: LanguageManagementToolbarActionContributors =
      this.injector.get(LANGUAGE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: LanguageManagementEntityPropContributors =
      this.injector.get(LANGUAGE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: LanguageManagementCreateFormPropContributors =
      this.injector.get(LANGUAGE_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: LanguageManagementEditFormPropContributors =
      this.injector.get(LANGUAGE_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};
    if (!isFlagIconEnabled) {
      this.filterFlagIcon(
        DEFAULT_LANGUAGE_MANAGEMENT_ENTITY_PROPS,
        DEFAULT_LANGUAGE_MANAGEMENT_CREATE_FORM_PROPS,
        DEFAULT_LANGUAGE_MANAGEMENT_EDIT_FORM_PROPS,
      );
    }

    return getObjectExtensionEntitiesFromStore(this.injector, 'LanguageManagement').pipe(
      map(entities => ({
        [eLanguageManagementComponents.Languages]: entities.Language,
      })),
      mapEntitiesToContributors(this.injector, 'LanguageManagement'),
      tap(objectExtensionContributors => {
        mergeWithDefaultActions(
          extensions.entityActions,
          DEFAULT_LANGUAGE_MANAGEMENT_ENTITY_ACTIONS,
          actionContributors,
        );
        mergeWithDefaultActions(
          extensions.toolbarActions,
          DEFAULT_LANGUAGE_MANAGEMENT_TOOLBAR_ACTIONS,
          toolbarContributors,
        );
        mergeWithDefaultProps(
          extensions.entityProps,
          DEFAULT_LANGUAGE_MANAGEMENT_ENTITY_PROPS,
          objectExtensionContributors.prop,
          propContributors,
        );
        mergeWithDefaultProps(
          extensions.createFormProps,
          DEFAULT_LANGUAGE_MANAGEMENT_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors,
        );
        mergeWithDefaultProps(
          extensions.editFormProps,
          DEFAULT_LANGUAGE_MANAGEMENT_EDIT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      mapTo(true),
    );
  }

  private filterFlagIcon(
    ...defaults: Array<{
      [eLanguageManagementComponents.Languages]: Array<
        EntityProp<LanguageDto> | FormProp<LanguageDto>
      >;
    }>
  ) {
    defaults.forEach(d => {
      d[eLanguageManagementComponents.Languages] = d[
        eLanguageManagementComponents.Languages
      ].filter(prop => prop.name !== 'flagIcon');
    });
  }
}
