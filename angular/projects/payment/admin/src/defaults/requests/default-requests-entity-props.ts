import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import {
  paymentRequestStateOptions,
  PaymentRequestWithDetailsDto,
} from '@volo/abp.ng.payment/proxy';
import { SubscriptionTooltipComponent } from '../../components/subscription-tooltip/subscription-tooltip.component';
import { getSubscriptionStatus } from '../../utils/get-subscription-status';

export const DEFAULT_PAYMENT_REQUESTS_ENTITY_PROPS =
  EntityProp.createMany<PaymentRequestWithDetailsDto>([
    {
      type: ePropType.DateTime,
      name: 'creationTime',
      displayName: 'Payment::DisplayName:CreationTime',
      sortable: true,
    },
    {
      type: ePropType.Number,
      name: 'totalPrice',
      displayName: 'Payment::DisplayName:TotalPrice',
      sortable: true,
      component: SubscriptionTooltipComponent,
      valueResolver: data =>
        getSubscriptionStatus(!!data.record.externalSubscriptionId, data.record.totalPrice),
    },
    {
      type: ePropType.String,
      name: 'currency',
      displayName: 'Payment::DisplayName:Currency',
    },
    {
      type: ePropType.Enum,
      name: 'state',
      displayName: 'Payment::DisplayName:State',
      sortable: true,
      enumList: paymentRequestStateOptions,
    },
    {
      type: ePropType.String,
      name: 'gateway',
      displayName: 'Payment::DisplayName:Gateway',
      sortable: true,
    },
    {
      type: ePropType.String,
      name: 'externalSubscriptionId',
      displayName: 'Payment::DisplayName:ExternalSubscriptionId',
    },
  ]);
