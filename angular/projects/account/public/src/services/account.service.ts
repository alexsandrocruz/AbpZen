import { PagedResultDto, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import {
  IdentitySecurityLogDto,
  IdentitySecurityLogGetListInput,
} from '@volo/abp.commercial.ng.ui/config';
import { Observable } from 'rxjs';
import type {
  ConfirmEmailInput,
  ConfirmPhoneNumberInput,
  GetTwoFactorProvidersInput,
  IFormFile,
  ProfilePictureInput,
  ProfilePictureSourceDto,
  RegisterDto,
  ResetPasswordDto,
  SendEmailConfirmationTokenDto,
  SendPasswordResetCodeDto,
  SendPhoneNumberConfirmationTokenDto,
  SendTwoFactorCodeInput,
} from '../models/account';
import { UrlComponentEncoder } from './url-component-encoder';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiName = 'AbpAccountPublic';

  confirmEmail = (input: ConfirmEmailInput) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/account/confirm-email`,
        body: input,
      },
      { apiName: this.apiName },
    );

  confirmPhoneNumber = (input: ConfirmPhoneNumberInput) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/account/confirm-phone-number`,
        body: input,
      },
      { apiName: this.apiName },
    );

  getProfilePicture = (id: string) =>
    this.restService.request<any, ProfilePictureSourceDto>(
      {
        method: 'GET',
        url: `/api/account/profile-picture/${id}`,
      },
      { apiName: this.apiName },
    );

  getProfilePictureFile = (id: string) =>
    this.restService.request<any, any>(
      {
        method: 'GET',
        url: `/api/account/profile-picture-file/${id}`,
      },
      { apiName: this.apiName },
    );

  register = (input: RegisterDto) =>
    this.restService.request<any, any>(
      {
        method: 'POST',
        url: `/api/account/register`,
        body: input,
      },
      { apiName: this.apiName },
    );

  resetPassword = (input: ResetPasswordDto) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/account/reset-password`,
        body: input,
      },
      { apiName: this.apiName },
    );

  sendEmailConfirmationToken = (input: SendEmailConfirmationTokenDto) =>
    this.restService.request<SendEmailConfirmationTokenDto, void>(
      {
        method: 'POST',
        url: `/api/account/send-email-confirmation-token`,
        body: input,
      },
      { apiName: this.apiName },
    );

  sendPasswordResetCode = (input: SendPasswordResetCodeDto) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/account/send-password-reset-code`,
        body: input,
      },
      { apiName: this.apiName },
    );

  sendPhoneNumberConfirmationToken = (input: SendPhoneNumberConfirmationTokenDto) =>
    this.restService.request<SendPhoneNumberConfirmationTokenDto, void>(
      {
        method: 'POST',
        url: `/api/account/send-phone-number-confirmation-token`,
        body: input,
      },
      { apiName: this.apiName },
    );

  setProfilePicture = (input: ProfilePictureInput) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/account/profile-picture`,
        body: input,
      },
      { apiName: this.apiName },
    );

  uploadProfilePictureFile = (image: IFormFile) =>
    this.restService.request<any, any>(
      {
        method: 'POST',
        url: `/api/account/profile-picture-file`,
      },
      { apiName: this.apiName },
    );

  getMySecurityLogsByInput(
    params = {} as Partial<IdentitySecurityLogGetListInput>,
  ): Observable<PagedResultDto<IdentitySecurityLogDto>> {
    return this.restService.request(
      { url: '/api/identity/security-logs/my', method: 'GET', params },
      { apiName: this.apiName },
    );
  }

  recaptchaByCaptchaResponse = (captchaResponse: string) =>
    this.restService.request<any, void>(
      {
        method: 'GET',
        url: '/api/account/recaptcha-validate',
        params: { captchaResponse },
      },
      { apiName: this.apiName },
    );

  getTwoFactorProviders = (input: GetTwoFactorProvidersInput) => {
    // related angular issue for encoder https://github.com/angular/angular/issues/11058
    return this.restService.request<void, string[]>(
      {
        url: '/api/account/two-factor-providers',
        method: 'GET',
        params: input,
      },
      { apiName: this.apiName, httpParamEncoder: new UrlComponentEncoder() },
    );
  };

  sendTwoFactorCode = (input: SendTwoFactorCodeInput) => {
    return this.restService.request<SendTwoFactorCodeInput, void>(
      {
        url: '/api/account/send-two-factor-code',
        method: 'POST',
        body: input,
      },
      { apiName: this.apiName },
    );
  };

  constructor(private restService: RestService) {}
}
