import type { ChatContactDto, GetContactsInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  apiName = 'Chat';
  

  getContacts = (input: GetContactsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ChatContactDto[]>({
      method: 'GET',
      url: '/api/chat/contact/contacts',
      params: { filter: input.filter, includeOtherContacts: input.includeOtherContacts },
    },
    { apiName: this.apiName,...config });
  

  getTotalUnreadMessageCount = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/chat/contact/total-unread-message-count',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
