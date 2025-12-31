import { ToolbarAction } from '@abp/ng.components/extensible';
import { PlanDto } from '@volo/abp.ng.payment/proxy';
import { PaymentPlansComponent } from '../../components/plans/plans.component';

export const DEFAULT_PLANS_TOOLBAR_ACTIONS = ToolbarAction.createMany<PlanDto[]>([
  {
    text: 'Payment::NewPlan',
    action: data => {
      const component = data.getInjected(PaymentPlansComponent);
      component.onAdd();
    },
    permission: 'Payment.Plans.Create',
    icon: 'fa fa-plus',
  },
]);
