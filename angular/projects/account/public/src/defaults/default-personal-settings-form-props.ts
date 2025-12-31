import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';
import { ProfileDto } from '@volo/abp.ng.account/public/proxy';
import { PersonalSettingsHalfRowComponent } from '../components/personal-settings/personal-settings-half-row/personal-settings-half-row.component';
import { PersonalSettingsEmailComponent } from '../components/personal-settings/personal-settings-email/personal-settings-email.component';
import { ConfigStateService } from '@abp/ng.core';
import { PersonalSettingsPhoneNumberComponent } from '../components/personal-settings/personal-settings-phone-number/personal-settings-phone-number.component';
import { PersonalSettingsTimerZoneComponent } from '../components';

const { maxLength, required, email } = Validators;
export const DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS = FormProp.createMany<ProfileDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::DisplayName:UserName',
    id: 'username',
    validators: () => [required, maxLength(256)],
    readonly: data => {
      const configState = data.getInjected(ConfigStateService);
      const settings = configState.getSettings();
      return !(
        (settings['Abp.Identity.User.IsUserNameUpdateEnabled'] || '').toLowerCase() !== 'false'
      );
    },
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::DisplayName:Name',
    id: 'name',
    validators: () => [maxLength(64)],
    template: PersonalSettingsHalfRowComponent,
    className: 'd-inline-block w-50',
  },
  {
    type: ePropType.String,
    name: 'surname',
    displayName: 'AbpIdentity::DisplayName:Surname',
    id: 'surname',
    validators: () => [maxLength(64)],
    className: 'd-inline-block w-50 ps-4',
    template: PersonalSettingsHalfRowComponent,
  },
  {
    type: ePropType.String,
    name: 'email',
    displayName: 'AbpIdentity::DisplayName:Email',
    id: 'email-address',
    validators: () => [required, email, maxLength(256)],
    template: PersonalSettingsEmailComponent,
    readonly: data => {
      const configState = data.getInjected(ConfigStateService);
      const settings = configState.getSettings();
      return !(
        (settings['Abp.Identity.User.IsEmailUpdateEnabled'] || '').toLowerCase() !== 'false'
      );
    },
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::DisplayName:PhoneNumber',
    id: 'phone-number',
    validators: () => [maxLength(16)],
    template: PersonalSettingsPhoneNumberComponent,
  },
  {
    type: ePropType.String,
    name: 'timezone',
    displayName: 'AbpSettingManagement::Menu:TimeZone',
    id: 'timezone',
    template: PersonalSettingsTimerZoneComponent,
    visible: data => {
      const configState = data.getInjected(ConfigStateService);
      const kind = configState.getDeep('clock.kind');

      return kind === 'Utc';
    },
  },
]);
