import type { ChatMessageSide } from './chat-message-side.enum';

export interface ChatMessageDto {
  id?: string;
  message?: string;
  messageDate?: string;
  isRead: boolean;
  readDate?: string;
  side: ChatMessageSide;
}

export interface DeleteMessageInput {
  targetUserId?: string;
  messageId?: string;
}

export interface SendMessageInput {
  targetUserId?: string;
  message: string;
}
