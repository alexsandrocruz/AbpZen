import type {
  EntityDto,
  ExtensibleObject,
  ExtensiblePagedAndSortedResultRequestDto,
} from '@abp/ng.core';
import type { ProfilePictureType } from './profile-picture-type.enum';
import type { IRemoteStreamContent } from './volo/abp/content/models';

export interface AuthenticatorInfoDto {
  key?: string;
  uri?: string;
}

export interface ChangePasswordInput {
  currentPassword?: string;
  newPassword: string;
}

export interface ConfirmEmailInput {
  userId: string;
  token: string;
}

export interface ConfirmPhoneNumberInput {
  userId: string;
  token: string;
}

export interface DelegateNewUserInput {
  targetUserId?: string;
  startTime: string;
  endTime: string;
}

export interface GetAccountIdentitySessionListInput
  extends ExtensiblePagedAndSortedResultRequestDto {
  device?: string;
  clientId?: string;
}

export interface GetTwoFactorProvidersInput {
  userId: string;
  token: string;
}

export interface GetUserLookupInput {
  userName?: string;
}

export interface IdentityUserConfirmationStateDto {
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
}

export interface IsLinkedInput {
  userId?: string;
  tenantId?: string;
}

export interface LinkUserDto {
  targetUserId?: string;
  targetUserName?: string;
  targetTenantId?: string;
  targetTenantName?: string;
  directlyLinked: boolean;
}

export interface LinkUserInput {
  userId?: string;
  tenantId?: string;
  token: string;
}

export interface ProfileDto extends ExtensibleObject {
  userName?: string;
  email?: string;
  emailConfirmed: boolean;
  name?: string;
  surname?: string;
  phoneNumber?: string;
  phoneNumberConfirmed: boolean;
  isExternal: boolean;
  hasPassword: boolean;
  supportsMultipleTimezone: boolean;
  timezone?: string;
  concurrencyStamp?: string;
}

export interface ProfilePictureInput {
  type: ProfilePictureType;
  imageContent: IRemoteStreamContent;
}

export interface ProfilePictureSourceDto {
  type: ProfilePictureType;
  source?: string;
  fileContent: number[];
}

export interface RegisterDto extends ExtensibleObject {
  userName: string;
  emailAddress: string;
  password: string;
  appName: string;
  returnUrl?: string;
  returnUrlHash?: string;
  captchaResponse?: string;
}

export interface ResetPasswordDto {
  userId?: string;
  resetToken: string;
  password: string;
}

export interface SendEmailConfirmationTokenDto {
  appName: string;
  userId: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface SendPasswordResetCodeDto {
  email: string;
  appName: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface SendPhoneNumberConfirmationTokenDto {
  userId: string;
  phoneNumber?: string;
}

export interface SendTwoFactorCodeInput {
  userId: string;
  provider: string;
  token: string;
}

export interface UnLinkUserInput {
  userId?: string;
  tenantId?: string;
}

export interface UpdateProfileDto extends ExtensibleObject {
  userName: string;
  email?: string;
  name?: string;
  surname?: string;
  phoneNumber?: string;
  timezone?: string;
  concurrencyStamp?: string;
}

export interface UserDelegationDto extends EntityDto<string> {
  userName?: string;
  startTime?: string;
  endTime?: string;
}

export interface UserLookupDto extends EntityDto<string> {
  userName?: string;
}

export interface VerifyAuthenticatorCodeDto {
  recoveryCodes: string[];
}

export interface VerifyAuthenticatorCodeInput {
  code?: string;
}

export interface VerifyEmailConfirmationTokenInput {
  userId: string;
  token: string;
}

export interface VerifyLinkLoginTokenInput {
  userId: string;
  tenantId?: string;
  token: string;
}

export interface VerifyLinkTokenInput {
  userId: string;
  tenantId?: string;
  token: string;
}

export interface VerifyPasswordResetTokenInput {
  userId?: string;
  resetToken: string;
}
