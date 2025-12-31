import { Validators } from '@angular/forms';
import { getPasswordValidators } from '@abp/ng.theme.shared';
import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';

export const DEFAULT_USERS_CREATE_FORM_PROPS = FormProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    id: 'user-name',
    validators: () => [Validators.required, Validators.maxLength(256)],
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::DisplayName:Name',
    id: 'name',
    group: { name: 'left', className: 'col col-md-6' },
    validators: () => [Validators.maxLength(64)],
  },
  {
    type: ePropType.String,
    name: 'surname',
    displayName: 'AbpIdentity::DisplayName:Surname',
    id: 'surname',
    group: { name: 'right', className: 'col col-md-6' },
    validators: () => [Validators.maxLength(64)],
  },
  {
    type: ePropType.PasswordInputGroup,
    name: 'password',
    displayName: 'AbpIdentity::Password',
    id: 'password',
    autocomplete: 'new-password',
    validators: data => [Validators.required, ...getPasswordValidators({ get: data.getInjected })],
  },
  {
    type: ePropType.Email,
    name: 'email',
    displayName: 'AbpIdentity::EmailAddress',
    id: 'email',
    validators: () => [Validators.required, Validators.maxLength(256), Validators.email],
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::PhoneNumber',
    id: 'phone-number',
    validators: () => [Validators.maxLength(16)],
  },
  {
    type: ePropType.Boolean,
    name: 'isActive',
    displayName: 'AbpIdentity::DisplayName:IsActive',
    id: 'active-checkbox',
    group: { name: 'isActive', className: 'col col-md-6' },
    defaultValue: true,
  },
  {
    type: ePropType.Boolean,
    name: 'lockoutEnabled',
    displayName: 'AbpIdentity::DisplayName:LockoutEnabled',
    id: 'lockout-checkbox',
    group: { name: 'lockout-checkbox', className: 'col col-md-6' },
    tooltip: { text: 'AbpIdentity::Description:LockoutEnabled' },
    defaultValue: true,
  },
  {
    type: ePropType.Boolean,
    name: 'emailConfirmed',
    displayName: 'AbpIdentity::DisplayName:EmailConfirmed',
    id: 'emailConfirmed-checkbox',
    group: { name: 'emailConfirmed', className: 'col col-md-6' },
    defaultValue: false,
  },
  {
    type: ePropType.Boolean,
    name: 'phoneNumberConfirmed',
    displayName: 'AbpIdentity::DisplayName:PhoneNumberConfirmed',
    id: 'phoneNumberConfirmed',
    group: { name: 'phoneNumberConfirmed', className: 'col col-md-6' },
    defaultValue: false,
  },
  {
    type: ePropType.Boolean,
    name: 'shouldChangePasswordOnNextLogin',
    displayName: 'AbpIdentity::DisplayName:ShouldChangePasswordOnNextLogin',
    id: 'shouldChangePasswordOnNextLogin',
    tooltip: { text: 'AbpIdentity::Description:ShouldChangePasswordOnNextLogin' },
    group: { name: 'shouldChangePasswordOnNextLogin', className: 'col col-md-6' },
    defaultValue: false,
  },
]);

export const DEFAULT_USERS_EDIT_FORM_PROPS = DEFAULT_USERS_CREATE_FORM_PROPS.filter(
  prop => prop.name !== 'password',
);
