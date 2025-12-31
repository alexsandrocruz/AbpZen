import { Injectable, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, combineLatest, filter, switchMap, tap } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import {
  AuthService,
  ConfigStateService,
  CurrentUserDto,
  EnvironmentService,
  PermissionService,
} from '@abp/ng.core';
import { NavItemsService, ToasterService } from '@abp/ng.theme.shared';
import { ContactService } from '@volo/abp.ng.chat/proxy';

const permission = 'Chat.Messaging';

export interface ChatMessage {
  senderUserId: string;
  senderUsername: string;
  senderName: string;
  senderSurname: string;
  text: string;
}

@Injectable({
  providedIn: 'root',
})
export class ChatConfigService {
  protected readonly toaster = inject(ToasterService);
  protected readonly router = inject(Router);
  protected readonly authService = inject(AuthService);
  protected readonly navItems = inject(NavItemsService);
  protected readonly permissionService = inject(PermissionService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly environmentService = inject(EnvironmentService);
  protected readonly contactService = inject(ContactService);

  protected readonly unreadMessagesCount = new BehaviorSubject<number>(0);

  apiName = 'Chat';
  conectedUserId: string;
  connection: signalR.HubConnection;
  message$ = new Subject<ChatMessage>();

  get unreadMessagesCount$(): Observable<number> {
    return this.unreadMessagesCount.asObservable();
  }

  get isChatEnabled(): boolean {
    return (this.configState.getFeature('Chat.Enable') || '').toLowerCase() !== 'false';
  }

  get signalRUrl() {
    const { apis } = this.environmentService.getEnvironment();
    return apis[this.apiName]?.signalRUrl || apis[this.apiName]?.url || apis.default.url;
  }

  protected listenToAppConfig(): void {
    this.configState
      .createOnUpdateStream(state => state)
      .pipe(
        filter(({ currentUser }) => {
          const { isAuthenticated } = currentUser || {};
          const isGranted = this.permissionService.getGrantedPolicy(permission);

          if ((!isAuthenticated || !isGranted) && this.connection) {
            this.connection.stop();
          }
          if (!this.isChatEnabled) {
            this.toggleChat();
          }
          if (isGranted) {
            this.initSignalR(currentUser as CurrentUserDto);
          }

          return isGranted;
        }),
        switchMap(() => this.setTotalUnreadMessageCount()),
        switchMap(() => combineLatest([this.listenToMessages(), this.listenToRouterEvents()])),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  protected toggleChat(): void {
    if (!this.isChatEnabled && this.connection) {
      this.connection.stop();
    }

    this.navItems.patchItem('Chat.ChatIconComponent', { visible: () => this.isChatEnabled });
  }

  protected initSignalR(currentUser: CurrentUserDto): void {
    if (this.conectedUserId === currentUser.id) {
      return;
    }

    if (this.connection) {
      this.connection.stop();
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.signalRUrl}/signalr-hubs/chat`, {
        accessTokenFactory: () => this.authService.getAccessToken(),
      })
      .build();

    this.connection.on('ReceiveMessage', message => this.message$.next(message));

    this.connection
      .start()
      .then(() => (this.conectedUserId = currentUser.id))
      .catch(err => console.error(err.toString()));
  }

  constructor() {
    this.listenToAppConfig();
  }

  setTotalUnreadMessageCount() {
    if (this.router.url === '/chat') return;

    return this.contactService
      .getTotalUnreadMessageCount()
      .pipe(tap(count => this.unreadMessagesCount.next(count)));
  }

  addUnreadMessageCount(count: number): void {
    if (!count || count < 1) {
      this.resetUnreadMessageCount();
      return;
    }

    const currentCount = this.unreadMessagesCount.getValue();
    this.unreadMessagesCount.next(currentCount + count);
  }

  resetUnreadMessageCount(): void {
    this.unreadMessagesCount.next(0);
  }

  listenToMessages(): Observable<ChatMessage> {
    return this.message$.pipe(
      filter(() => this.router.url !== '/chat'),
      tap(message => {
        const { senderName, senderSurname, senderUsername, text } = message;
        const sender = `${senderName ? senderName + (senderSurname ? ' ' + senderSurname : '') : senderUsername}`;
        const toasterMessage = `<strong>${sender}</strong>: ${
          text.length > 50 ? text.substring(0, 49) + '...' : text
        }`;

        this.addUnreadMessageCount(1);
        this.toaster.info(toasterMessage, null, {
          tapToDismiss: true,
          iconClass: 'bi-chat-dots',
        });
      }),
    );
  }

  listenToRouterEvents() {
    return this.router.events.pipe(
      filter(event => event instanceof NavigationEnd && event.url === '/chat'),
      tap(() => this.resetUnreadMessageCount()),
    );
  }
}
