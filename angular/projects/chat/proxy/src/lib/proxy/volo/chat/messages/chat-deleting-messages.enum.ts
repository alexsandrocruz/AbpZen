import { mapEnumToOptions } from '@abp/ng.core';

export enum ChatDeletingMessages {
  Enabled = 1,
  Disabled = 2,
  EnabledWithDeletionPeriod = 3,
}

export const chatDeletingMessagesOptions = mapEnumToOptions(ChatDeletingMessages);
