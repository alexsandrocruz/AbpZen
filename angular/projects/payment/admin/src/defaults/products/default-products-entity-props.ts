import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { PaymentType, paymentTypeOptions } from '@volo/abp.ng.payment/proxy';
import { SubscriptionTooltipComponent } from '../../components/subscription-tooltip/subscription-tooltip.component';
import { getSubscriptionStatus } from '../../utils/get-subscription-status';

export const DEFAULT_PAYMENT_PRODUCTS_ENTITY_PROPS = EntityProp.createMany([
  {
    type: ePropType.String,
    name: 'code',
    displayName: 'Payment::DisplayName:Code',
    sortable: false,
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'Payment::DisplayName:Name',
  },
  {
    type: ePropType.Number,
    name: 'count',
    displayName: 'Payment::DisplayName:Count',
  },
  {
    type: ePropType.Number,
    name: 'unitPrice',
    displayName: 'Payment::DisplayName:UnitPrice',
    component: SubscriptionTooltipComponent,
    valueResolver: data =>
      getSubscriptionStatus(
        data.record.paymentType === PaymentType.Subscription,
        data.record.unitPrice,
      ),
  },
  {
    type: ePropType.Number,
    name: 'totalPrice',
    displayName: 'Payment::DisplayName:TotalPrice',
    component: SubscriptionTooltipComponent,
    valueResolver: data =>
      getSubscriptionStatus(
        data.record.paymentType === PaymentType.Subscription,
        data.record.totalPrice,
      ),
  },
  {
    type: ePropType.Enum,
    name: 'paymentType',
    displayName: 'Payment::DisplayName:PaymentType',
    enumList: paymentTypeOptions,
  },
]);
