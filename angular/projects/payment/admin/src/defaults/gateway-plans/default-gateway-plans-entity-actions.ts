import { EntityAction } from '@abp/ng.components/extensible';
import { GatewayPlanDto } from '@volo/abp.ng.payment/proxy';
import { GatewayPlansComponent } from '../../components/gateway-plans/gateway-plans.component';

export const DEFAULT_GATEWAY_PLANS_ENTITY_ACTIONS = EntityAction.createMany<GatewayPlanDto>([
  {
    text: 'Payment::Edit',
    action: data => {
      const component = data.getInjected(GatewayPlansComponent);
      component.onEdit(data.record.gateway);
    },
    permission: 'Payment.Plans.GatewayPlans.Update',
  },
  {
    text: 'Payment::Delete',
    action: data => {
      const component = data.getInjected(GatewayPlansComponent);
      component.onDelete(data.record.gateway);
    },
    permission: 'Payment.Plans.GatewayPlans.Delete',
  },
]);
