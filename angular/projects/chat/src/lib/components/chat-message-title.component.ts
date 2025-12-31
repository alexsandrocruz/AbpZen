import { Component, Input } from '@angular/core';
import { ChatContactDto } from '@volo/abp.ng.chat/proxy';
import { getContactName } from './chat-contacts.component';

@Component({
  selector: 'abp-chat-message-title',
  template: `
    <div class="px-2 py-1 border mb-2 rounded-2">
      <div class="row">
        <div class="col-auto py-1 pe-0">
          <abp-conversation-avatar small [contact]="selectedContact"></abp-conversation-avatar>
        </div>
        <div class="col d-flex flex-column justify-content-center">
          <h5 id="conversation-title" class="m-0">{{ contactName }}</h5>
          <small id="conversation-info"></small>
        </div>
      </div>
    </div>
  `,
})
export class ChatMessageTitleComponent {
  @Input()
  selectedContact: ChatContactDto;

  get contactName(): string {
    return getContactName(this.selectedContact || ({} as ChatContactDto));
  }
}
