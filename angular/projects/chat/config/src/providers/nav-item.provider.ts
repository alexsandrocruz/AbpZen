import { APP_INITIALIZER, inject } from '@angular/core';
import { Router } from '@angular/router';
import { NavItemsService } from '@abp/ng.theme.shared';
import { ChatIconComponent } from '../components/chat-icon.component';
import { eChatPolicyNames } from '../enums/policy-names';
import { ChatConfigService } from '../services/chat-config.service';

export const CHAT_NAV_ITEM_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureNavItems,
    deps: [NavItemsService, Router],
    multi: true,
  },
];

export function configureNavItems(navItems: NavItemsService, router: Router) {
  const chatConfigService = inject(ChatConfigService);

  return () => {
    navItems.addItems([
      {
        id: 'Chat.ChatIconComponent',
        name: 'Chat::Feature:ChatGroup',
        requiredPolicy: eChatPolicyNames.Messaging,
        component: ChatIconComponent,
        badge: {
          count: chatConfigService.unreadMessagesCount$,
        },
        order: 99.99,
        icon: 'fas fa-comments fa-lg',
        action: () => {
          router.navigate(['/chat']);
        },
      },
    ]);
  };
}
