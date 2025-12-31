import { AfterViewInit, Component, DestroyRef, ElementRef, inject, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { fromEvent } from 'rxjs';
import { catchError, debounceTime, finalize } from 'rxjs/operators';
import { ConfigStateService, HttpWaitService } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import {
  ChatMessageSide,
  ChatContactDto,
  ChatMessageDto,
  ChatDeletingConversations,
  ChatDeletingMessages,
  ConversationService,
  chatDeletingConversationsOptions,
  chatDeletingMessagesOptions,
} from '@volo/abp.ng.chat/proxy';
import { ChatConfigService, ChatMessage } from '@volo/abp.ng.chat/config';
import { ChatContactsComponent } from './chat-contacts.component';

const now = new Date();
const today = new Date(now.getFullYear(), now.getMonth(), now.getDate()).valueOf();

@Component({
  selector: 'abp-chat',
  templateUrl: 'chat.component.html',
  styles: [
    `
      .chat-box {
        height: calc(100vh - 390px);
        overflow-y: scroll;
      }

      .message-text {
        white-space: pre-line;
      }

      .message-date {
        position: absolute;
        opacity: 0.35;
        width: 100px;
        top: 1px;
      }

      .message-date.left {
        left: -110px;
        text-align: right;
      }

      .message-date.right {
        right: -110px;
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
      .media-body:hover .down-arrow {
        opacity: 100%;
      }

      .chat-box-container {
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
export class ChatComponent implements AfterViewInit {
  readonly #destroyRef = inject(DestroyRef);
  protected readonly conversationService = inject(ConversationService);
  protected readonly chatConfigService = inject(ChatConfigService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly httpWaitService = inject(HttpWaitService);
  protected readonly confirmation = inject(ConfirmationService);

  selectedContact = {} as ChatContactDto;
  unreadMessageCount = 0;
  userMessages = new Map<string, ChatMessageDto[]>();
  chatMessageSide = ChatMessageSide;
  message: string;
  sendOnEnter: boolean;
  loading: boolean;
  pagingLoading: boolean;
  allMessagesLoaded: boolean;
  clickedStartMessage = false;
  conversationDeletable = false;
  messageDeletable = false;
  messageDeletableWithPeriod = false;
  messageDeletePeriod: number;

  @ViewChild('chatBox', { static: false })
  chatBoxRef: ElementRef<HTMLDivElement>;

  @ViewChild(ChatContactsComponent, { static: false })
  chatContactsComponent: ChatContactsComponent;

  get selectedContactMessages(): ChatMessageDto[] {
    return this.userMessages.get(this.selectedContact.userId) || [];
  }

  protected addFilterToHttpWaitService(): void {
    this.httpWaitService.addFilter([
      {
        method: 'POST',
        endpoint: '/api/chat/conversation/send-message',
      },
      {
        method: 'GET',
        endpoint: '/api/chat/contact/contacts',
      },
    ]);
  }

  protected listenToChatBoxScroll(): void {
    if (this.chatBoxRef) {
      fromEvent(this.chatBoxRef.nativeElement, 'scroll')
        .pipe(debounceTime(150), takeUntilDestroyed(this.#destroyRef))
        .subscribe(this.onScroll);
    }
  }

  protected listenToNewMessages(): void {
    this.chatConfigService.message$
      .pipe(takeUntilDestroyed(this.#destroyRef))
      .subscribe(({ senderUserId, text } = {} as ChatMessage) => {
        if (!this.userMessages.has(senderUserId)) {
          return;
        }

        const isSelected = this.selectedContact.userId === senderUserId;
        this.userMessages.get(senderUserId).push({
          message: text,
          messageDate: Date(),
          side: ChatMessageSide.Receiver,
          isRead: isSelected,
          readDate: isSelected ? Date() : null,
        });

        this.scrollToEnd();
      });
  }

  protected init() {
    this.configState.getSettings$('Volo.Chat.Messaging').subscribe(chatSetting => {
      const deletingMessages = chatDeletingMessagesOptions.find(
        opt => opt.key === chatSetting['Volo.Chat.Messaging.DeletingMessages'],
      ).value;
      const deletingConversations = chatDeletingConversationsOptions.find(
        opt => opt.key === chatSetting['Volo.Chat.Messaging.DeletingConversations'],
      ).value;

      this.conversationDeletable =
        deletingMessages === ChatDeletingMessages.Enabled &&
        deletingConversations === ChatDeletingConversations.Enabled;

      this.messageDeletable = deletingMessages === ChatDeletingMessages.Enabled;

      this.messageDeletableWithPeriod =
        deletingMessages === ChatDeletingMessages.EnabledWithDeletionPeriod;

      this.messageDeletePeriod = Number(chatSetting['Volo.Chat.Messaging.MessageDeletionPeriod']);
    });
  }

  constructor() {
    this.init();
    this.addFilterToHttpWaitService();
  }

  ngAfterViewInit(): void {
    this.sendOnEnter =
      (
        this.configState.getSetting('Volo.Chat.Messaging.SendMessageOnEnter') || ''
      ).toLowerCase() !== 'false';

    this.listenToChatBoxScroll();
    this.listenToNewMessages();
  }

  getConversation(scrollToEnd: boolean) {
    this.allMessagesLoaded = false;
    this.conversationService
      .getConversation({
        skipCount: 0,
        maxResultCount: 50,
        targetUserId: this.selectedContact.userId,
      })
      .subscribe(res => {
        this.userMessages.set(this.selectedContact.userId, res.messages.reverse());

        if (scrollToEnd) {
          this.scrollToEnd();
        }

        if (this.selectedContact.unreadMessageCount) {
          this.markConversationAsRead();
        }
      });
  }

  send() {
    if (!this.message || this.loading) return;

    this.unreadMessageCount = 0;
    this.loading = true;
    this.conversationService
      .sendMessage({ message: this.message, targetUserId: this.selectedContact.userId })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(message => {
        this.chatContactsComponent.changeLastMessageOfSelectedContact(this.message);

        if (!this.userMessages.has(this.selectedContact.userId)) {
          this.userMessages.set(this.selectedContact.userId, []);
        }
        this.userMessages.get(this.selectedContact.userId).push({
          ...message,
        });

        this.message = '';
        this.scrollToEnd();
      });
  }

  markConversationAsRead() {
    this.conversationService
      .markConversationAsRead({ targetUserId: this.selectedContact.userId })
      .subscribe(() => {
        this.chatContactsComponent.markSelectedContactAsRead();
        this.selectedContact.unreadMessageCount = 0;
        setTimeout(() => (this.unreadMessageCount = 0), 5000);
      });
  }

  sendWithEnter(event: KeyboardEvent) {
    if (!this.sendOnEnter) return;

    event.preventDefault();
    this.send();
  }

  onSelectContact(contact: ChatContactDto) {
    this.selectedContact = contact;
    this.unreadMessageCount = contact.unreadMessageCount;
    this.scrollToEnd();

    if (this.userMessages.has(contact.userId) || !contact.lastMessage) {
      if (contact.unreadMessageCount) this.markConversationAsRead();
      return;
    }

    this.getConversation(true);
  }

  getDateFormat(date: string | Date): string {
    date = new Date(date);
    const messageDay = new Date(date.getFullYear(), date.getMonth(), date.getDate()).valueOf();

    if (messageDay === today) return 'shortTime';

    return 'short';
  }

  scrollToEnd() {
    setTimeout(() => {
      const { offsetTop } = (document.querySelector('.unread-message-count-badge-wrapper') ||
        {}) as HTMLDivElement;

      this.chatBoxRef?.nativeElement.scrollTo({
        top: offsetTop ? offsetTop - 60 : this.chatBoxRef.nativeElement.scrollHeight,
      });
    }, 0);
  }

  onScroll = (event: Event) => {
    if (
      this.allMessagesLoaded ||
      this.pagingLoading ||
      !this.selectedContact.lastMessage ||
      this.chatBoxRef?.nativeElement.scrollTop > 250 ||
      this.selectedContactMessages.length % 50 !== 0
    ) {
      event.preventDefault();
      return;
    }

    this.pagingLoading = true;
    this.conversationService
      .getConversation({
        skipCount: this.selectedContactMessages.length,
        maxResultCount: 50,
        targetUserId: this.selectedContact.userId,
      })
      .pipe(finalize(() => (this.pagingLoading = false)))
      .subscribe(res => {
        if (!res.messages.length) {
          this.allMessagesLoaded = true;
          return;
        }
        this.userMessages.get(this.selectedContact.userId).unshift(...res.messages.reverse());
      });
  };

  startConversation() {
    this.clickedStartMessage = true;
  }

  deleteMessage(message: ChatMessageDto) {
    this.conversationService
      .deleteMessage({ targetUserId: this.selectedContact.userId, messageId: message.id })
      .pipe(
        catchError(err => {
          const error = err.error.error;
          if (error.code === 'Volo.Chat:010005') {
            if (error.data.seconds <= 0) {
              this.messageDeletableWithPeriod = false;
            } else {
              this.selectedContactMessages.find(msg => msg.id === message.id).readDate = null;
            }
          }

          throw err;
        }),
      )
      .subscribe({
        next: () => {
          const newArray = this.userMessages.get(this.selectedContact.userId).filter(val => {
            return val.id !== message.id;
          });
          this.userMessages.set(this.selectedContact.userId, newArray);
        },
        error: err => {
          const error = err.error?.error?.message;
          this.confirmation.warn(error, 'AbpUi::Warning', {
            hideCancelBtn: true,
            yesText: 'AbpUi::Ok',
          });
        },
      });
  }

  showDeleteDropdown(message: ChatMessageDto) {
    const readDate = new Date(message.readDate).getTime();
    const now = new Date().getTime();
    return now - readDate > this.messageDeletePeriod ? false : true;
  }

  conversationDeleted(contact: ChatContactDto) {
    if (this.selectedContact.userId === contact.userId) {
      this.selectedContact = {} as ChatContactDto;
      this.userMessages.delete(contact.userId);
    }
  }
}
