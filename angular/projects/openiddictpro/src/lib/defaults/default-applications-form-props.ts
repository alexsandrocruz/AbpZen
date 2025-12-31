import { Validators } from '@angular/forms';
import { map } from 'rxjs/operators';
import { LocalizationService } from '@abp/ng.core';
import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Scopes } from '@volo/abp.ng.openiddictpro/proxy';
import { Applications, ScopeService } from '@volo/abp.ng.openiddictpro/proxy';

import { UrisValidator } from '../utils/uris-validator';
import { defaultApplicationTypes, defaultApplicationTypesList } from './default-applications-types';
import { ApplicationFormModalComponent } from '../components/application-form-modal/application-form-modal.component';
import { allowFlow } from '../components/scopes/allow-flow';
import { of } from 'rxjs';

const groupLeft = {
  name: 'left',
  className: 'col col-md-6',
};
const groupRight = {
  name: 'right',
  className: 'col col-md-6',
};
const className = 'mb-1 form-group';
// @ts-ignore
export const DEFAULT_APPLICATIONS_CREATE_FORM_PROPS =
  FormProp.createMany<Applications.Dtos.ApplicationDto>([
    {
      type: ePropType.String,
      options: data => of(defaultApplicationTypesList),
      name: 'applicationType',
      displayName: 'AbpOpenIddict::ApplicationType',
      id: 'applicationType',
      group: groupLeft,
      className: className,
      validators: () => [Validators.required],
    },

    {
      type: ePropType.String,
      name: 'clientId',
      displayName: 'AbpOpenIddict::ClientId',
      id: 'clientId',
      validators: () => [Validators.required],
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'displayName',
      displayName: 'AbpOpenIddict::DisplayName',
      id: 'displayName',
      validators: () => [Validators.required],
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'clientUri',
      displayName: 'AbpOpenIddict::ClientUri',
      id: 'clientUri',
      validators: () => [UrisValidator()],
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'logoUri',
      displayName: 'AbpOpenIddict::LogoUri',
      id: 'logoUri',
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'clientType',
      displayName: 'AbpOpenIddict::ClientType',
      id: 'clientType',
      validators: () => [Validators.required],
      options: data => {
        const key = data.getInjected(LocalizationService).instant('AbpUi::NotAssigned');
        return data
          .getInjected(ApplicationFormModalComponent)
          .types$.pipe(map(val => [{ key, value: null }, ...val]));
      },
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'clientSecret',
      displayName: 'AbpOpenIddict::ClientSecret',
      id: 'clientSecret',
      visible: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return formValue.clientType === defaultApplicationTypes.confidential;
      },
      group: groupLeft,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowAuthorizationCodeFlow',
      displayName: 'AbpOpenIddict::AllowAuthorizationCodeFlow',
      id: 'allowAuthorizationCodeFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowImplicitFlow',
      displayName: 'AbpOpenIddict::AllowImplicitFlow',
      id: 'allowImplicitFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowHybridFlow',
      displayName: 'AbpOpenIddict::AllowHybridFlow',
      id: 'allowHybridFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowPasswordFlow',
      displayName: 'AbpOpenIddict::AllowPasswordFlow',
      id: 'allowPasswordFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowClientCredentialsFlow',
      displayTextResolver: data => {
        return data
          .getInjected(ApplicationFormModalComponent)
          .changeTextToType('AbpOpenIddict::AllowClientCredentialsFlow');
      },
      disabled: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return formValue.type === defaultApplicationTypes.public;
      },

      id: 'allowClientCredentialsFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowRefreshTokenFlow',
      displayName: 'AbpOpenIddict::AllowRefreshTokenFlow',
      disabled: data => {
        const { allowHybridFlow, allowAuthorizationCodeFlow, allowPasswordFlow } = data
          .getInjected(ApplicationFormModalComponent)
          .getFormValue();
        return !(allowHybridFlow || allowAuthorizationCodeFlow || allowPasswordFlow);
      },
      id: 'allowRefreshTokenFlow',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowDeviceEndpoint',
      displayTextResolver: data => {
        return data
          .getInjected(ApplicationFormModalComponent)
          .changeTextToType('AbpOpenIddict::AllowDeviceEndpoint');
      },
      disabled: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return formValue.type === defaultApplicationTypes.public;
      },

      id: 'allowDeviceEndpoint',
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.String,
      name: 'consentType',
      displayName: 'AbpOpenIddict::ConsentType',
      id: 'consentType',
      options: data => data.getInjected(ApplicationFormModalComponent).consentTypes$,
      visible: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return allowFlow(formValue);
      },
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Text,
      name: 'extensionGrantTypes',
      displayName: 'AbpOpenIddict::ExtensionGrantTypes',
      id: 'extensionGrantTypes',
      className: className,
      group: groupRight,
    },
    {
      type: ePropType.MultiSelect,
      name: 'scopes',
      displayName: 'AbpOpenIddict::Scopes',
      id: 'scopes',
      defaultValue: [],
      options: data => {
        return data
          .getInjected(ScopeService)
          .getAllScopes()
          .pipe(
            map((result: Scopes.Dtos.ScopeDto[]) =>
              result.map(scope => ({
                key: scope.name,
                value: scope.name,
              })),
            ),
          );
      },
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Text,
      name: 'redirectUris',
      displayName: 'AbpOpenIddict::RedirectUris',
      id: 'redirectUris',
      validators: () => [UrisValidator()],
      visible: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return allowFlow(formValue);
      },
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Boolean,
      defaultValue: false,
      name: 'allowLogoutEndpoint',
      displayName: 'AbpOpenIddict::AllowLogoutEndpoint',
      id: 'allowLogoutEndpoint',
      visible: data => {
        const formValue = data.getInjected(ApplicationFormModalComponent).getFormValue();
        return allowFlow(formValue);
      },
      group: groupRight,
      className: className,
    },
    {
      type: ePropType.Text,
      name: 'postLogoutRedirectUris',
      displayName: 'AbpOpenIddict::PostLogoutRedirectUris',
      id: 'postLogoutRedirectUris',
      validators: () => [UrisValidator()],
      visible: data => {
        const { allowLogoutEndpoint } = data
          .getInjected(ApplicationFormModalComponent)
          .getFormValue();
        return allowLogoutEndpoint;
      },
      group: groupRight,
      className: className,
    },
  ]);

export const DEFAULT_APPLICATIONS_FORM_PROPS = DEFAULT_APPLICATIONS_CREATE_FORM_PROPS;
