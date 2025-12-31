import type { ChatDeletingMessages } from '../messages/chat-deleting-messages.enum';
import type { ChatDeletingConversations } from '../messages/chat-deleting-conversations.enum';
export interface ChatSettingsDto {
  deletingMessages: ChatDeletingMessages;
  messageDeletionPeriod: number;
  deletingConversations: ChatDeletingConversations;
}

export interface SendOnEnterSettingDto {
  sendOnEnter: boolean;
}
