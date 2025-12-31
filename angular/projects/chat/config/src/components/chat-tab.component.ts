import { ConfigStateService, CoreModule, isNullOrUndefined, TrackByService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  ChatDeletingConversationsNames,
  ChatDeletingMessagesNames,
} from '../enums/delete-message.enums';
import { DeleteConversationModel, DeleteMessageModel } from '../models/deleteMessage.model';
import {
  ChatDeletingMessages,
  ChatDeletingConversations,
  ChatSettingsDto,
  SettingsService,
} from '@volo/abp.ng.chat/proxy';
import { finalize } from 'rxjs';
import { messageDeletionValidator } from '../validators/message-deletion.validator';

@Component({
  standalone: true,
  selector: 'abp-chat-tab',
  templateUrl: './chat-tab.component.html',
  imports: [CoreModule, ThemeSharedModule],
})
export class ChatTabComponent {
  protected readonly toasterService = inject(ToasterService);
  protected readonly chatSettingsService = inject(SettingsService);
  protected readonly configStateService = inject(ConfigStateService);
  protected readonly fb = inject(FormBuilder);
  public readonly track = inject(TrackByService);

  loading = false;

  deleteMessageForm = this.fb.group(
    {
      deletingMessages: [ChatDeletingMessages.Enabled],
      deletingConversations: [ChatDeletingConversations.Enabled],
      messageDeletionPeriod: [0, [Validators.min(0), Validators.max(Number.MAX_SAFE_INTEGER)]],
    },
    {
      validators: messageDeletionValidator(),
    },
  );

  protected init() {
    this.chatSettingsService.get().subscribe(response => {
      this.deleteMessageForm.setValue(response);
    });
  }
  constructor() {
    this.init();
  }

  deleteMessageOptions: DeleteMessageModel[] = [
    { name: ChatDeletingMessagesNames.Enabled, value: ChatDeletingMessages.Enabled },
    { name: ChatDeletingMessagesNames.Disabled, value: ChatDeletingMessages.Disabled },
    {
      name: ChatDeletingMessagesNames.WithPeriod,
      value: ChatDeletingMessages.EnabledWithDeletionPeriod,
    },
  ];

  deleteConversationOptions: DeleteConversationModel[] = [
    { name: ChatDeletingConversationsNames.Enabled, value: ChatDeletingConversations.Enabled },
    { name: ChatDeletingConversationsNames.Disabled, value: ChatDeletingConversations.Disabled },
  ];

  get deletingMessages() {
    return this.deleteMessageForm.value.deletingMessages;
  }

  get deletingConversations() {
    return this.deleteMessageForm.value.deletingConversations;
  }

  get messageDeletionPeriod() {
    return this.deleteMessageForm.value.messageDeletionPeriod;
  }

  onSubmit() {
    if (this.deleteMessageForm.invalid) {
      return;
    }

    const input = { ...this.deleteMessageForm.value } as ChatSettingsDto;
    this.loading = true;

    if (isNullOrUndefined(input.messageDeletionPeriod)) {
      input.messageDeletionPeriod = 0;
    }

    this.chatSettingsService
      .update(input)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.configStateService.refreshAppState();
      });
  }
}
