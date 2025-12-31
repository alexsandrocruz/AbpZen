import type { ChatSettingsDto, SendOnEnterSettingDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  apiName = 'Chat';
  
  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ChatSettingsDto>({
      method: 'GET',
      url: '/api/chat/settings',
    },
    { apiName: this.apiName,...config });
  

  setSendOnEnterSetting = (input: SendOnEnterSettingDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/chat/settings/send-on-enter',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (input: ChatSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/chat/settings',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
