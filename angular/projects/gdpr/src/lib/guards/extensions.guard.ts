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
  DEFAULT_GDPR_ENTITY_PROPS,
  DEFAULT_GDPR_TOOLBAR_ACTIONS,
} from '../tokens/extensions.token';
import { eGdprComponents } from '../enums/components';
import { GdprEntityPropContributors, GdprToolbarActionContributors } from '../models';

/**
 * @deprecated Use `gdprExtensionsResolver` *function* instead.
 */
@Injectable()
export class GdprExtensionsGuard {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const propContributors: GdprEntityPropContributors =
      this.injector.get(DEFAULT_GDPR_ENTITY_PROPS, null) || {};
    const toolbarContributors: GdprToolbarActionContributors =
      this.injector.get(DEFAULT_GDPR_TOOLBAR_ACTIONS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'Gdpr').pipe(
      map(entities => ({
        [eGdprComponents.PersonalData]: entities.PersonalData,
      })),
      mapEntitiesToContributors(this.injector, 'AbpGdpr'),
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
      map(() => true),
    );
  }
}
