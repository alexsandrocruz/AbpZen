import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.components/extensible';
import { GatewayDto, GatewayPlanDto, PlanDto } from '@volo/abp.ng.payment/proxy';
import { ePaymentComponents } from '../enums/components';

export type PaymentEntityActionContributors = Partial<{
  [ePaymentComponents.Plans]: EntityActionContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EntityActionContributorCallback<GatewayPlanDto>[];
}>;

export type PaymentToolbarActionContributors = Partial<{
  [ePaymentComponents.Plans]: ToolbarActionContributorCallback<PlanDto[]>[];
  [ePaymentComponents.GatewayPlans]: ToolbarActionContributorCallback<GatewayDto[]>[];
}>;

export type PaymentEntityPropContributors = Partial<{
  [ePaymentComponents.Plans]: EntityPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EntityPropContributorCallback<GatewayPlanDto>[];
}>;

export type PaymentCreateFormPropContributors = Partial<{
  [ePaymentComponents.Plans]: CreateFormPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: CreateFormPropContributorCallback<GatewayDto>[];
}>;

export type PaymentEditFormPropContributors = Partial<{
  [ePaymentComponents.Plans]: EditFormPropContributorCallback<PlanDto>[];
  [ePaymentComponents.GatewayPlans]: EditFormPropContributorCallback<GatewayDto>[];
}>;

export interface PaymentConfigOptions {
  entityActionContributors?: PaymentEntityActionContributors;
  toolbarActionContributors?: PaymentToolbarActionContributors;
  entityPropContributors?: PaymentEntityPropContributors;
  createFormPropContributors?: PaymentCreateFormPropContributors;
  editFormPropContributors?: PaymentEditFormPropContributors;
}
