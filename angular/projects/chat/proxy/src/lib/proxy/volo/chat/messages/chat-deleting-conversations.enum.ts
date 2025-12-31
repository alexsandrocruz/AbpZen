import { mapEnumToOptions } from '@abp/ng.core';

export enum ChatDeletingConversations {
  Enabled = 1,
  Disabled = 2,
}

export const chatDeletingConversationsOptions = mapEnumToOptions(ChatDeletingConversations);
