import { ToolbarAction } from '@abp/ng.components/extensible';
import { GatewayDto } from '@volo/abp.ng.payment/proxy';
import { GatewayPlansComponent } from '../../components/gateway-plans/gateway-plans.component';

export const DEFAULT_GATEWAY_PLANS_TOOLBAR_ACTIONS = ToolbarAction.createMany<GatewayDto[]>([
  {
    text: 'Payment::NewGatewayPlan',
    action: data => {
      const component = data.getInjected(GatewayPlansComponent);
      component.onAdd();
    },
    permission: 'Payment.Plans.GatewayPlans.Create',
    icon: 'fa fa-plus',
  },
]);
