import { EntityAction } from '@abp/ng.components/extensible';
import { PlanDto } from '@volo/abp.ng.payment/proxy';
import { PaymentPlansComponent } from '../../components/plans/plans.component';

export const DEFAULT_PLANS_ENTITY_ACTIONS = EntityAction.createMany<PlanDto>([
  {
    text: 'Payment::GatewayPlans',
    action: data => {
      const component = data.getInjected(PaymentPlansComponent);
      component.goToGatewayPlans(data.record.id);
    },
    permission: 'Payment.Plans.GatewayPlans',
  },
  {
    text: 'Payment::Edit',
    action: data => {
      const component = data.getInjected(PaymentPlansComponent);
      component.onEdit(data.record.id);
    },
    permission: 'Payment.Plans.Update',
  },
  {
    text: 'Payment::Delete',
    action: data => {
      const component = data.getInjected(PaymentPlansComponent);
      component.onDelete(data.record.id, data.record.name);
    },
    permission: 'Payment.Plans.Delete',
  },
]);
