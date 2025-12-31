import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { InjectionToken } from '@angular/core';
import {
  GatewayDto,
  GatewayPlanDto,
  PaymentRequestProductDto,
  PaymentRequestWithDetailsDto,
  PlanDto,
} from '@volo/abp.ng.payment/proxy';
import { DEFAULT_GATEWAY_PLANS_ENTITY_ACTIONS } from '../defaults/gateway-plans/default-gateway-plans-entity-actions';
import { DEFAULT_GATEWAY_PLANS_ENTITY_PROPS } from '../defaults/gateway-plans/default-gateway-plans-entity-props';
import {
  DEFAULT_GATEWAY_PLANS_CREATE_FORM_PROPS,
  DEFAULT_GATEWAY_PLANS_EDIT_FORM_PROPS,
} from '../defaults/gateway-plans/default-gateway-plans-form-props';
import { DEFAULT_GATEWAY_PLANS_TOOLBAR_ACTIONS } from '../defaults/gateway-plans/default-gateway-plans-toolbar-actions';
import { DEFAULT_PLANS_ENTITY_ACTIONS } from '../defaults/plans/default-plans-entity-actions';
import { DEFAULT_PLANS_ENTITY_PROPS } from '../defaults/plans/default-plans-entity-props';
import {
  DEFAULT_PLANS_CREATE_FORM_PROPS,
  DEFAULT_PLANS_EDIT_FORM_PROPS,
} from '../defaults/plans/default-plans-form-props';
import { DEFAULT_PLANS_TOOLBAR_ACTIONS } from '../defaults/plans/default-plans-toolbar-actions';
import { DEFAULT_PAYMENT_PRODUCTS_ENTITY_PROPS } from '../defaults/products';
import { DEFAULT_PAYMENT_REQUESTS_ENTITY_ACTIONS } from '../defaults/requests/default-requests-entity-actions';
import { DEFAULT_PAYMENT_REQUESTS_ENTITY_PROPS } from '../defaults/requests/default-requests-entity-props';
import { ePaymentComponents } from '../enums/components';

export const DEFAULT_PAYMENT_ENTITY_ACTIONS = {
  [ePaymentComponents.Plans]: DEFAULT_PLANS_ENTITY_ACTIONS,
  [ePaymentComponents.GatewayPlans]: DEFAULT_GATEWAY_PLANS_ENTITY_ACTIONS,
  [ePaymentComponents.Requests]: DEFAULT_PAYMENT_REQUESTS_ENTITY_ACTIONS,
};

export const DEFAULT_PAYMENT_TOOLBAR_ACTIONS = {
  [ePaymentComponents.Plans]: DEFAULT_PLANS_TOOLBAR_ACTIONS,
  [ePaymentComponents.GatewayPlans]: DEFAULT_GATEWAY_PLANS_TOOLBAR_ACTIONS,
};

export const DEFAULT_PAYMENT_ENTITY_PROPS = {
  [ePaymentComponents.Plans]: DEFAULT_PLANS_ENTITY_PROPS,
  [ePaymentComponents.GatewayPlans]: DEFAULT_GATEWAY_PLANS_ENTITY_PROPS,
  [ePaymentComponents.Products]: DEFAULT_PAYMENT_PRODUCTS_ENTITY_PROPS,
  [ePaymentComponents.Requests]: DEFAULT_PAYMENT_REQUESTS_ENTITY_PROPS,
};

export const DEFAULT_PAYMENT_CREATE_FORM_PROPS = {
  [ePaymentComponents.Plans]: DEFAULT_PLANS_CREATE_FORM_PROPS,
  [ePaymentComponents.GatewayPlans]: DEFAULT_GATEWAY_PLANS_CREATE_FORM_PROPS,
};

export const DEFAULT_PAYMENT_EDIT_FORM_PROPS = {
  [ePaymentComponents.Plans]: DEFAULT_PLANS_EDIT_FORM_PROPS,
  [ePaymentComponents.GatewayPlans]: DEFAULT_GATEWAY_PLANS_EDIT_FORM_PROPS,
};

export const PAYMENT_ENTITY_ACTION_CONTRIBUTORS = new InjectionToken<EntityActionContributors>(
  'PAYMENT_ENTITY_ACTION_CONTRIBUTORS',
);

export const PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS = new InjectionToken<ToolbarActionContributors>(
  'PAYMENT_TOOLBAR_ACTION_CONTRIBUTORS',
);

export const PAYMENT_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
  'PAYMENT_ENTITY_PROP_CONTRIBUTORS',
);

export const PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS = new InjectionToken<CreateFormPropContributors>(
  'PAYMENT_CREATE_FORM_PROP_CONTRIBUTORS',
);

export const PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS = new InjectionToken<EditFormPropContributors>(
  'PAYMENT_EDIT_FORM_PROP_CONTRIBUTORS',
);

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [ePaymentComponents.Plans]: EntityActionContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EntityActionContributorCallback<GatewayPlanDto>[];
  [ePaymentComponents.Requests]: EditFormPropContributorCallback<PaymentRequestWithDetailsDto>[];
}>;
type ToolbarActionContributors = Partial<{
  [ePaymentComponents.Plans]: ToolbarActionContributorCallback<PlanDto[]>[];
  [ePaymentComponents.GatewayPlans]: ToolbarActionContributorCallback<GatewayDto[]>[];
}>;
type EntityPropContributors = Partial<{
  [ePaymentComponents.Plans]: EntityPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EntityPropContributorCallback<GatewayPlanDto>[];
  [ePaymentComponents.Requests]: EntityPropContributorCallback<PaymentRequestWithDetailsDto>[];
  [ePaymentComponents.Products]: EntityPropContributorCallback<PaymentRequestProductDto>[];
}>;
type CreateFormPropContributors = Partial<{
  [ePaymentComponents.Plans]: CreateFormPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: CreateFormPropContributorCallback<GatewayDto>[];
}>;
type EditFormPropContributors = Partial<{
  [ePaymentComponents.Plans]: EditFormPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EditFormPropContributorCallback<GatewayDto>[];
}>;
