import type { ChatMessageDto } from '../messages/models';
import type { ChatTargetUserInfo } from '../users/models';
import type { PagedResultRequestDto } from '@abp/ng.core';

export interface ChatConversationDto {
  messages: ChatMessageDto[];
  targetUserInfo: ChatTargetUserInfo;
}

export interface DeleteConversationInput {
  targetUserId?: string;
}

export interface GetConversationInput extends PagedResultRequestDto {
  targetUserId?: string;
}

export interface MarkConversationAsReadInput {
  targetUserId?: string;
}
