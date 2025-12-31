import {
  AuthGuard,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GatewayPlansComponent } from './components/gateway-plans/gateway-plans.component';
import { PaymentPlansComponent } from './components/plans/plans.component';
import { ProductsComponent } from './components/products/products.component';
import { RequestsComponent } from './components/requests/requests.component';
import { ePaymentComponents } from './enums/components';
import { PaymentInitializer } from './payment.initializer';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'plans',
    pathMatch: 'full',
  },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [AuthGuard, PermissionGuard, PaymentInitializer],
    children: [
      {
        path: 'plans',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Payment.Plans',
          replaceableComponent: {
            key: ePaymentComponents.Plans,
            defaultComponent: PaymentPlansComponent,
          } as ReplaceableComponents.RouteData<PaymentPlansComponent>,
        },
      },
      {
        path: 'plans/:planId',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Payment.Plans.GatewayPlans',
          replaceableComponent: {
            key: ePaymentComponents.GatewayPlans,
            defaultComponent: GatewayPlansComponent,
          } as ReplaceableComponents.RouteData<GatewayPlansComponent>,
        },
      },
      {
        path: 'requests',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Payment.PaymentRequests',
          replaceableComponent: {
            key: ePaymentComponents.Requests,
            defaultComponent: RequestsComponent,
          },
        },
      },
      {
        path: 'requests/:requestId/products',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'Payment.PaymentRequests',
          replaceableComponent: {
            key: ePaymentComponents.Products,
            defaultComponent: ProductsComponent,
          },
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class PaymentAdminRoutingModule {}
