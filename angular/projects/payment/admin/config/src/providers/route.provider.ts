import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { ePaymentRouteNames } from '../enums/route-names';
import { ePaymentPolicyNames } from '../enums/policy-names';

export const PAYMENT_ROUTE_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureRoutes,
    deps: [RoutesService],
    multi: true,
  },
];

export function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: undefined,
        name: ePaymentRouteNames.Payment,
        // TODO: let people know about this number
        order: 50,
        layout: eLayoutType.application,
        iconClass: 'fa fa-credit-card',
      },
      {
        path: '/payment/plans',
        name: ePaymentRouteNames.PaymentPlans,
        parentName: ePaymentRouteNames.Payment,
        order: 1,
        requiredPolicy: ePaymentPolicyNames.PaymentPlans,
      },
      {
        path: '/payment/requests',
        name: ePaymentRouteNames.Requests,
        parentName: ePaymentRouteNames.Payment,
        order: 2,
        requiredPolicy: ePaymentPolicyNames.PaymentRequests,
      },
    ]);
  };
}
