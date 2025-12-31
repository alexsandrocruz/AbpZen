import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  effect,
  inject,
  model,
  output,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter, take } from 'rxjs';
import { ChatContactDto, ContactService, ConversationService } from '@volo/abp.ng.chat/proxy';
import { ChatConfigService, ChatMessage } from '@volo/abp.ng.chat/config';
import { ChatMessageSide } from '../enums/chat-message-side';

@Component({
  selector: 'abp-chat-contacts',
  templateUrl: 'chat-contacts.component.html',
  styles: [
    `
      .messages-box {
        overflow-y: scroll;
        max-height: calc(100vh - 254.34px);
      }
      .dropdown-toggle::after {
        display: none;
      }
      .dropdown {
        border-color: transparent;
      }
      .down-arrow {
        border: none;
        opacity: 0;
      }
      .media:hover .down-arrow {
        opacity: 100%;
      }

      .list-group {
        padding-right: 7px;
      }
      ::-webkit-scrollbar {
        width: 5px;
      }

      ::-webkit-scrollbar-thumb {
        background: var(--lpx-border-color);
      }
    `,
  ],
})
export class ChatContactsComponent implements OnInit {
  protected readonly contactService = inject(ContactService);
  protected readonly chatConfigService = inject(ChatConfigService);
  protected readonly chatConversationService = inject(ConversationService);

  @Input() conversationDeletable: boolean;

  startConversation = model<boolean | undefined>();
  #startConversation: boolean | undefined;

  @Output()
  selected = new EventEmitter<ChatContactDto>();
  conversationDeleted = output<ChatContactDto>();

  allContacts: ChatContactDto[] = [];
  selectedContact = {} as ChatContactDto;
  filter = '';

  getContactName = getContactName;

  get contacts(): ChatContactDto[] {
    return this.allContacts
      .filter(contact => contact.lastMessage)
      .sort((a, b) =>
        new Date(a.lastMessageDate).valueOf() > new Date(b.lastMessageDate).valueOf() ? -1 : 1,
      );
  }

  get otherContacts(): ChatContactDto[] {
    return this.allContacts.filter(contact => !contact.lastMessage);
  }

  protected listenToNewMessages() {
    this.chatConfigService.message$
      .pipe(takeUntilDestroyed())
      .subscribe((message = {} as ChatMessage) => {
        const index = this.allContacts.findIndex(cont => cont.userId === message.senderUserId);

        const contact = {
          userId: message.senderUserId,
          username: message.senderUsername,
          name: message.senderName,
          surname: message.senderSurname,
          lastMessage: message.text,
          unreadMessageCount: 1,
          lastMessageDate: new Date().toString(),
          lastMessageSide: ChatMessageSide.Receiver,
        } as ChatContactDto;

        if (index > -1) {
          let unreadMessageCount = this.allContacts[index].unreadMessageCount + 1;
          if (this.selectedContact.userId === this.allContacts[index].userId) {
            unreadMessageCount = 0;
          }

          this.allContacts[index] = { ...contact, unreadMessageCount };
          return;
        }

        if (!this.filter) {
          this.allContacts.push(contact);
        }
      });
  }

  constructor() {
    this.listenToNewMessages();
    this.contactService
      .getTotalUnreadMessageCount()
      .pipe(filter(Boolean))
      .subscribe(count => {
        if (count > 0) {
          this.get(null, false);
        }
      });

    effect(() => {
      if (this.startConversation()) {
        this.#startConversation = this.startConversation();
        this.filter = undefined;
        this.get(null, true);
      }
    });
  }

  ngOnInit() {
    if (this.allContacts.length) {
      this.selectContact(this.allContacts[0]);
    }
  }

  typing() {
    if (!this.filter && this.#startConversation) {
      this.get(null, true);
      return;
    }

    if (this.filter) {
      this.get(() => {
        this.startConversation.set(false);
      }, true);
      return;
    }

    if (!this.filter && !this.#startConversation) {
      this.allContacts = [];
    }
  }

  get(onSuccess?: Function, includeOtherContacts?: boolean) {
    const isBoolean = typeof includeOtherContacts === 'boolean';
    const includeOtherContactsParam = isBoolean ? includeOtherContacts : !!this.filter;

    this.filter = this.filter?.trim();

    this.contactService
      .getContacts({
        includeOtherContacts: includeOtherContactsParam,
        filter: this.filter,
      })
      .subscribe(res => {
        this.allContacts = res;
        if (onSuccess) {
          onSuccess();
        }
      });
  }

  selectContact(contact: ChatContactDto) {
    this.selectedContact = contact;
    this.selected.emit(contact);
  }

  changeLastMessageOfSelectedContact(message: string) {
    this.selectedContact.lastMessage = message;
    this.selectedContact.lastMessageDate = new Date().toString();

    const index = this.allContacts.findIndex(
      contact => contact.userId === this.selectedContact.userId,
    );

    this.allContacts[index] = this.selectedContact;
  }

  markSelectedContactAsRead() {
    this.selectedContact.unreadMessageCount = 0;
  }

  deleteConversation(contact: ChatContactDto) {
    this.chatConversationService
      .deleteConversation({ targetUserId: contact.userId })
      .pipe(take(1))
      .subscribe(() => {
        this.conversationDeleted.emit(contact);
        this.selectedContact = null;
        this.get(null, true);
      });
  }
}

export function getContactName({ name, surname, username }: ChatContactDto) {
  if (!name && !surname) return username;

  return `${name || ''} ${surname || ''}`;
}
