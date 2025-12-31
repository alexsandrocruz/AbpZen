import { ChatDeletingConversationsNames, ChatDeletingMessagesNames } from "../enums/delete-message.enums";
import { ChatDeletingConversations, ChatDeletingMessages } from "@volo/abp.ng.chat/proxy";

export interface DeleteMessageModel{
  name:ChatDeletingMessagesNames,
  value:ChatDeletingMessages
}
export interface DeleteConversationModel{
  name:ChatDeletingConversationsNames,
  value:ChatDeletingConversations
}