import { Component, inject } from '@angular/core';
import { ChatConfigService } from '../services/chat-config.service';

@Component({
  selector: 'abp-chat-icon',
  template: `
    <div
      class="position-relative"
      placement="left"
      [ngbTooltip]="'Chat::Menu:Chat' | abpLocalization"
    >
      <a>
        <i class="fas fa-comments fa-lg"></i>
        @if (chatConfigService.unreadMessagesCount$ | async; as messageCount) {
          <span class="badge text-dark">
            {{ messageCount }}
          </span>
        }
      </a>
    </div>
  `,
  styles: [
    `
      a {
        color: inherit;
      }

      .badge {
        position: absolute;
        right: -10px;
        top: -8px;
      }
    `,
  ],
})
export class ChatIconComponent {
  protected readonly chatConfigService = inject(ChatConfigService);
}
