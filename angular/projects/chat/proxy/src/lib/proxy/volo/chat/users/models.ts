import type { ChatMessageSide } from '../messages/chat-message-side.enum';

export interface ChatContactDto {
  userId?: string;
  name?: string;
  surname?: string;
  username?: string;
  hasChatPermission: boolean;
  lastMessageSide: ChatMessageSide;
  lastMessage?: string;
  lastMessageDate?: string;
  unreadMessageCount: number;
}

export interface ChatTargetUserInfo {
  userId?: string;
  name?: string;
  surname?: string;
  username?: string;
}

export interface GetContactsInput {
  filter?: string;
  includeOtherContacts: boolean;
}
