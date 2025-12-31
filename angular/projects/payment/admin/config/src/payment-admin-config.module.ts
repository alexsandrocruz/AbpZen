import { NgModule, ModuleWithProviders } from '@angular/core';
import { PAYMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule({})
export class PaymentAdminConfigModule {
  static forRoot(): ModuleWithProviders<PaymentAdminConfigModule> {
    return {
      ngModule: PaymentAdminConfigModule,
      providers: [PAYMENT_ROUTE_PROVIDERS],
    };
  }
}
