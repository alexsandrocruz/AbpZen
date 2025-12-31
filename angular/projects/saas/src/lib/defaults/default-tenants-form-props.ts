import { Validators } from '@angular/forms';
import { LocalizationService } from '@abp/ng.core';
import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { SaasTenantDto, TenantActivationState } from '@volo/abp.ng.saas/proxy';
import { of } from 'rxjs';
import { TenantsComponent } from '../components/tenants/tenants.component';

export const DEFAULT_TENANTS_CREATE_FORM_PROPS = FormProp.createMany<SaasTenantDto>([
  {
    type: ePropType.String,
    name: 'name',
    id: 'name',
    displayName: 'Saas::TenantName',
    validators: () => [Validators.required, Validators.maxLength(256)],
  },
  {
    type: ePropType.String,
    name: 'editionId',
    displayName: 'Saas::Edition',
    id: 'edition',
    disabled: data => {
      const editionEndDate = data.record.editionEndDateUtc;
      const subscriptionExpired =
        editionEndDate && new Date(editionEndDate).getTime() < new Date().getTime();
      return subscriptionExpired;
    },
    options: data => {
      const editions = data.getInjected(TenantsComponent).editions;
      const localization = data.getInjected(LocalizationService);
      const emptyOption = {
        value: null,
        key: localization.instant('AbpUi::NotAssigned'),
      };

      return of([
        emptyOption,
        ...editions.map(edition => ({
          key: edition.displayName,
          value: edition.id,
        })),
      ]);
    },
  },
  {
    type: ePropType.Email,
    name: 'adminEmailAddress',
    displayName: 'Saas::DisplayName:AdminEmailAddress',
    id: 'admin-email-address',
    validators: () => [Validators.required, Validators.maxLength(256), Validators.email],
    formText: 'Saas::DisplayName:AdminEmailAddressFormText',
  },
  {
    type: ePropType.PasswordInputGroup,
    name: 'adminPassword',
    displayName: 'Saas::DisplayName:AdminPassword',
    id: 'admin-password',
    autocomplete: 'new-password',
    validators: () => [Validators.required],
  },
  {
    type: ePropType.String,
    name: 'activationState',
    displayName: 'Saas::DisplayName:ActivationState',
    id: 'activation-state',
    validators: () => [Validators.required],
    defaultValue: TenantActivationState.Active,
    options: data => {
      const localization = data.getInjected(LocalizationService);

      return of([
        {
          key: localization.instant('Saas::Enum:TenantActivationState.Active'),
          value: TenantActivationState.Active,
        },
        {
          key: localization.instant('Saas::Enum:TenantActivationState.ActiveWithLimitedTime'),
          value: TenantActivationState.ActiveWithLimitedTime,
        },
        {
          key: localization.instant('Saas::Enum:TenantActivationState.Passive'),
          value: TenantActivationState.Passive,
        },
      ]);
    },
  },
  {
    type: ePropType.DateTime,
    name: 'activationEndDate',
    displayName: 'Saas::DisplayName:ActivationEndDate',
    id: 'activation-end-date',
    visible: data => {
      const { tenantForm } = data.getInjected(TenantsComponent);
      return (
        tenantForm.get('activationState').value === TenantActivationState.ActiveWithLimitedTime
      );
    },
  },
]);

export const DEFAULT_TENANTS_EDIT_FORM_PROPS = [
  ...DEFAULT_TENANTS_CREATE_FORM_PROPS.slice(0, 2),
  ...DEFAULT_TENANTS_CREATE_FORM_PROPS.slice(-2),
];
