import type { AuthenticatorInfoDto, ConfirmEmailInput, ConfirmPhoneNumberInput, GetTwoFactorProvidersInput, IdentityUserConfirmationStateDto, ProfilePictureInput, ProfilePictureSourceDto, RegisterDto, ResetPasswordDto, SendEmailConfirmationTokenDto, SendPasswordResetCodeDto, SendPhoneNumberConfirmationTokenDto, SendTwoFactorCodeInput, VerifyAuthenticatorCodeDto, VerifyAuthenticatorCodeInput, VerifyEmailConfirmationTokenInput, VerifyPasswordResetTokenInput } from './models';
import type { GetIdentitySecurityLogListInput, IdentitySecurityLogDto, IdentityUserDto } from './volo/abp/identity/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiName = 'AbpAccountPublic';
  

  confirmEmail = (input: ConfirmEmailInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/confirm-email',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  confirmPhoneNumber = (input: ConfirmPhoneNumberInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/confirm-phone-number',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getAuthenticatorInfo = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuthenticatorInfoDto>({
      method: 'GET',
      url: '/api/account/authenticator-info',
    },
    { apiName: this.apiName,...config });
  

  getConfirmationState = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserConfirmationStateDto>({
      method: 'GET',
      url: '/api/account/confirmation-state',
      params: { id },
    },
    { apiName: this.apiName,...config });
  

  getProfilePicture = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProfilePictureSourceDto>({
      method: 'GET',
      url: `/api/account/profile-picture/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getProfilePictureFile = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/account/profile-picture-file/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getSecurityLogList = (input: GetIdentitySecurityLogListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<IdentitySecurityLogDto>>({
      method: 'GET',
      url: '/api/account/security-logs',
      params: { startTime: input.startTime, endTime: input.endTime, applicationName: input.applicationName, identity: input.identity, action: input.action, userName: input.userName, clientId: input.clientId, correlationId: input.correlationId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount, extraProperties: input.extraProperties },
    },
    { apiName: this.apiName,...config });
  

  getTwoFactorProviders = (input: GetTwoFactorProvidersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>({
      method: 'GET',
      url: '/api/account/two-factor-providers',
      params: { userId: input.userId, token: input.token },
    },
    { apiName: this.apiName,...config });
  

  hasAuthenticator = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: '/api/account/has-authenticator-key',
    },
    { apiName: this.apiName,...config });
  

  recaptchaByCaptchaResponse = (captchaResponse: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: '/api/account/recaptcha-validate',
      params: { captchaResponse },
    },
    { apiName: this.apiName,...config });
  

  register = (input: RegisterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IdentityUserDto>({
      method: 'POST',
      url: '/api/account/register',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  resetAuthenticator = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/reset-authenticator',
    },
    { apiName: this.apiName,...config });
  

  resetPassword = (input: ResetPasswordDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/reset-password',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendEmailConfirmationToken = (input: SendEmailConfirmationTokenDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/send-email-confirmation-token',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendPasswordResetCode = (input: SendPasswordResetCodeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/send-password-reset-code',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendPhoneNumberConfirmationToken = (input: SendPhoneNumberConfirmationTokenDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/send-phone-number-confirmation-token',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendTwoFactorCode = (input: SendTwoFactorCodeInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/send-two-factor-code',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  setProfilePicture = (input: ProfilePictureInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/profile-picture',
      params: { type: input.type },
      body: input.imageContent,
    },
    { apiName: this.apiName,...config });
  

  verifyAuthenticatorCode = (input: VerifyAuthenticatorCodeInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VerifyAuthenticatorCodeDto>({
      method: 'POST',
      url: '/api/account/verify-authenticator-code',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  verifyEmailConfirmationToken = (input: VerifyEmailConfirmationTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/verify-email-confirmation-token',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  verifyPasswordResetToken = (input: VerifyPasswordResetTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/verify-password-reset-token',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
