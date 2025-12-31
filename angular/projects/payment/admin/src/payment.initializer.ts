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
import {
  PaymentCreateFormPropContributors,
  PaymentEditFormPropContributors,
  PaymentEntityActionContributors,
  PaymentEntityPropContributors,
  PaymentToolbarActionContributors,
} from './models/config-options';
import {
  DEFAULT_PAYMENT_CREATE_FORM_PROPS,
  DEFAULT_PAYMENT_EDIT_FORM_PROPS,
  DEFAULT_PAYMENT_ENTITY_ACTIONS,
  DEFAULT_PAYMENT_ENTITY_PROPS,
  DEFAULT_PAYMENT_TOOLBAR_ACTIONS,
  PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  PAYMENT_ENTITY_ACTION_CONTRIBUTORS,
  PAYMENT_ENTITY_PROP_CONTRIBUTORS,
  PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';

@Injectable()
export class PaymentInitializer {
  private readonly injector = inject(Injector);

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: PaymentEntityActionContributors =
      this.injector.get(PAYMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: PaymentToolbarActionContributors =
      this.injector.get(PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: PaymentEntityPropContributors =
      this.injector.get(PAYMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: PaymentCreateFormPropContributors =
      this.injector.get(PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: PaymentEditFormPropContributors =
      this.injector.get(PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'Payment').pipe(
      map(entities => ({
        // TODO: find out following entity name
        // [ePaymentComponents.Plans]: entities.PaymentPlans,
      })),
      mapEntitiesToContributors(this.injector, 'Payment'),
      tap(objectExtensionContributors => {
        mergeWithDefaultActions(
          extensions.entityActions,
          DEFAULT_PAYMENT_ENTITY_ACTIONS,
          actionContributors,
        );
        mergeWithDefaultActions(
          extensions.toolbarActions,
          DEFAULT_PAYMENT_TOOLBAR_ACTIONS,
          toolbarContributors,
        );
        mergeWithDefaultProps(
          extensions.entityProps,
          DEFAULT_PAYMENT_ENTITY_PROPS,
          objectExtensionContributors.prop,
          propContributors,
        );
        mergeWithDefaultProps(
          extensions.createFormProps,
          DEFAULT_PAYMENT_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors,
        );
        mergeWithDefaultProps(
          extensions.editFormProps,
          DEFAULT_PAYMENT_EDIT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      mapTo(true),
    );
  }
}
