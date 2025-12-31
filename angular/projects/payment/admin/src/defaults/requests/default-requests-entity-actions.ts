import { EntityAction } from '@abp/ng.components/extensible';
import { PaymentRequestWithDetailsDto } from '@volo/abp.ng.payment/proxy';
import { RequestsComponent } from '../../components/requests/requests.component';

export const DEFAULT_PAYMENT_REQUESTS_ENTITY_ACTIONS =
  EntityAction.createMany<PaymentRequestWithDetailsDto>([
    {
      text: 'Payment::Products',
      action: data => {
        const component = data.getInjected(RequestsComponent);
        component.goToProducts(data.record.id);
      },
      permission: 'Payment.PaymentRequests',
    },
  ]);
