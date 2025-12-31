import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';
import { GatewayDto, GatewayService } from '@volo/abp.ng.payment/proxy';
import { map } from 'rxjs/operators';

const externalIdField = {
  type: ePropType.String,
  name: 'externalId',
  displayName: 'Payment::DisplayName:ExternalId',
  id: 'external-id',
  validators: () => [Validators.required],
};

export const DEFAULT_GATEWAY_PLANS_CREATE_FORM_PROPS = FormProp.createMany<GatewayDto>([
  {
    type: ePropType.String,
    name: 'gateway',
    displayName: 'Payment::DisplayName:Gateway',
    id: 'gateway',
    options: data => {
      const service = data.getInjected(GatewayService);
      return service.getSubscriptionSupportedGateways().pipe(
        map(options =>
          options.map(opt => ({
            key: opt.displayName,
            value: opt.name,
          })),
        ),
      );
    },
    validators: () => [Validators.required],
  },
  externalIdField,
]);

export const DEFAULT_GATEWAY_PLANS_EDIT_FORM_PROPS = FormProp.createMany<GatewayDto>([
  {
    type: ePropType.String,
    name: 'gateway',
    displayName: 'Payment::DisplayName:Gateway',
    id: 'gateway',
    validators: () => [Validators.required],
    disabled: () => true,
  },
  externalIdField,
]);
