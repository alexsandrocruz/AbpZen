import { NgModule, ModuleWithProviders, NgModuleFactory } from '@angular/core';
import { LazyModuleFactory } from '@abp/ng.core';
import { PaymentAdminRoutingModule } from './payment-admin-routing.module';
import { PaymentInitializer } from './payment.initializer';
import {
  PAYMENT_ENTITY_ACTION_CONTRIBUTORS,
  PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS,
  PAYMENT_ENTITY_PROP_CONTRIBUTORS,
  PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS,
} from './tokens/extensions.token';
import { PaymentConfigOptions } from './models/config-options';
import { PaymentPlansModule } from './components/plans/plans.module';
import { GatewayPlansModule } from './components/gateway-plans/gateway-plans.module';
import { RequestsModule } from './components/requests/requests.module';
import { ProductsModule } from './components/products/products.module';

@NgModule({
  imports: [
    PaymentAdminRoutingModule,
    PaymentPlansModule,
    GatewayPlansModule,
    RequestsModule,
    ProductsModule,
  ],
  declarations: [],
})
export class PaymentAdminModule {
  static forChild(options: PaymentConfigOptions = {}): ModuleWithProviders<PaymentAdminModule> {
    return {
      ngModule: PaymentAdminModule,
      providers: [
        {
          provide: PAYMENT_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: PAYMENT_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        PaymentInitializer,
      ],
    };
  }

  static forLazy(options: PaymentConfigOptions = {}): NgModuleFactory<PaymentAdminModule> {
    return new LazyModuleFactory(PaymentAdminModule.forChild(options));
  }
}
