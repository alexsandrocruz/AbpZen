import type { IsLinkedInput, LinkUserDto, LinkUserInput, UnLinkUserInput, VerifyLinkLoginTokenInput, VerifyLinkTokenInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityLinkUserService {
  apiName = 'AbpAccountPublic';
  

  generateLinkLoginToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/account/link-user/generate-link-login-token',
    },
    { apiName: this.apiName,...config });
  

  generateLinkToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/account/link-user/generate-link-token',
    },
    { apiName: this.apiName,...config });
  

  getAllList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LinkUserDto>>({
      method: 'GET',
      url: '/api/account/link-user',
    },
    { apiName: this.apiName,...config });
  

  isLinked = (input: IsLinkedInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/link-user/is-linked',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  link = (input: LinkUserInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/link-user/link',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  unlink = (input: UnLinkUserInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/account/link-user/unlink',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  verifyLinkLoginToken = (input: VerifyLinkLoginTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/link-user/verify-link-login-token',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  verifyLinkToken = (input: VerifyLinkTokenInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/account/link-user/verify-link-token',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
