import type { ChangePasswordInput, ProfileDto, UpdateProfileDto } from './models';
import type { NameValue } from './volo/abp/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  apiName = 'AbpAccountPublic';

  canEnableTwoFactor = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>(
      {
        method: 'GET',
        url: '/api/account/my-profile/can-enable-two-factor',
      },
      { apiName: this.apiName, ...config },
    );

  changePassword = (input: ChangePasswordInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/my-profile/change-password',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProfileDto>(
      {
        method: 'GET',
        url: '/api/account/my-profile',
      },
      { apiName: this.apiName, ...config },
    );

  getTimezones = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, NameValue[]>(
      {
        method: 'GET',
        url: '/api/account/my-profile/timezones',
      },
      { apiName: this.apiName, ...config },
    );

  getTwoFactorEnabled = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>(
      {
        method: 'GET',
        url: '/api/account/my-profile/two-factor-enabled',
      },
      { apiName: this.apiName, ...config },
    );

  setTwoFactorEnabled = (enabled: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/my-profile/set-two-factor-enabled',
        params: { enabled },
      },
      { apiName: this.apiName, ...config },
    );

  update = (input: UpdateProfileDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProfileDto>(
      {
        method: 'PUT',
        url: '/api/account/my-profile',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
