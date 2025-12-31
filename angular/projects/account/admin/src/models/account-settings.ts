import { eTwoFactorBehaviour } from '../enums/two-factor-behaviour';

export interface AccountSettings {
  isSelfRegistrationEnabled: boolean;
  enableLocalLogin: boolean;
}

export interface AccountTwoFactorSettingsDto {
  twoFactorBehaviour: eTwoFactorBehaviour;
  isRememberBrowserEnabled: boolean;
  usersCanChange: boolean;
}

export interface AccountCaptchaSettings {
  useCaptchaOnLogin: boolean;
  useCaptchaOnRegistration: boolean;
  verifyBaseUrl: string;
  siteKey: string;
  siteSecret: string;
  version: number;
  score: number;
}

export interface AccountExternalProviderSetting {
  name: string;
  enabled: boolean;
  properties: {
    name: string;
    value: string;
  }[];
  secretProperties: {
    name: string;
    value: string;
  }[];
  // for the tenants' UI only
  useHostSettings?: boolean;
}

export interface AccountExternalProviderSettings {
  verifyPasswordDuringExternalLogin: boolean;
  externalProviders: AccountExternalProviderSetting[];
}
