import type { ChatConversationDto, DeleteConversationInput, GetConversationInput, MarkConversationAsReadInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ChatMessageDto, DeleteMessageInput, SendMessageInput } from '../messages/models';

@Injectable({
  providedIn: 'root',
})
export class ConversationService {
  apiName = 'Chat';
  

  deleteConversation = (input: DeleteConversationInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/chat/conversation/delete-conversation',
      params: { targetUserId: input.targetUserId },
    },
    { apiName: this.apiName,...config });
  

  deleteMessage = (input: DeleteMessageInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/chat/conversation/delete-message',
      params: { targetUserId: input.targetUserId, messageId: input.messageId },
    },
    { apiName: this.apiName,...config });
  

  getConversation = (input: GetConversationInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ChatConversationDto>({
      method: 'GET',
      url: '/api/chat/conversation/conversation',
      params: { targetUserId: input.targetUserId, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  markConversationAsRead = (input: MarkConversationAsReadInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/chat/conversation/mark-conversation-as-read',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  sendMessage = (input: SendMessageInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ChatMessageDto>({
      method: 'POST',
      url: '/api/chat/conversation/send-message',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
