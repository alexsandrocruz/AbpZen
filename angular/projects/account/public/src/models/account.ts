import { LoginParams } from '@abp/ng.core';
import { eProfilePictureType } from '@volo/abp.commercial.ng.ui/config';

export interface ConfirmEmailInput {
  userId: string;
  token: string;
}

export interface ConfirmPhoneNumberInput {
  token: string;
  userId: string;
}

export interface ProfilePictureInput {
  type: eProfilePictureType;
  imageContent: number[];
}

export interface ProfilePictureSourceDto {
  type: eProfilePictureType;
  source?: string;
  fileContent: number[];
}

export interface RegisterDto {
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
  email: string;
  userId: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface SendPhoneNumberConfirmationTokenDto {
  phoneNumber: string;
  userId: string;
}

export interface SendPasswordResetCodeDto {
  email: string;
  appName: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface IFormFile {
  contentType: string;
  contentDisposition: string;
  headers: any;
  length: number;
  name: string;
  fileName: string;
}

export interface GetTwoFactorProvidersInput {
  userId: string;
  token: string;
}

export interface SendTwoFactorCodeInput {
  userId: string;
  provider: string;
  token: string;
}

export interface RecoveryCodeData extends LoginParams {
  userId: string;
}

export type AuthenticatorSteps = 'Authenticator' | 'VerifyTheAuthenticator' | 'RecoveryCodes';
