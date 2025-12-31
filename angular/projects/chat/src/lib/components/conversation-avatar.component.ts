import { Component, Input } from '@angular/core';
import { ChatContactDto } from '@volo/abp.ng.chat/proxy';

const avatarStyles = [
  { color: '#ffffff', backgroundColor: '#3cb160' },
  { color: '#ffffff', backgroundColor: '#c373cc' },
  { color: '#ffffff', backgroundColor: '#2b78b3' },
  { color: '#ffffff', backgroundColor: '#6ac79a' },
  { color: '#ffffff', backgroundColor: '#aeb140' },
  { color: '#ffffff', backgroundColor: '#b773c0' },
  { color: '#ffffff', backgroundColor: '#e16d7a' },
  { color: '#ffffff', backgroundColor: '#ffac2a' },
  { color: '#ffffff', backgroundColor: '#21bbc7' },
  { color: '#ffffff', backgroundColor: '#59ab95' },
];

@Component({
  selector: 'abp-conversation-avatar',
  template: `
    <div
      class="avatar me-2 float-start rounded-circle"
      [class.small]="isSmall"
      [ngStyle]="avatarBgStyle"
    >
      {{ avatarText }}
    </div>
  `,
  styles: [
    `
      .avatar {
        height: 48px;
        width: 48px;
        line-height: 48px;
        font-size: 16px;
        font-weight: 700;
        text-align: center;
        text-transform: uppercase;
      }

      .avatar.small {
        height: 36px;
        width: 36px;
        line-height: 37px;
        font-size: 14px;
      }
    `,
  ],
})
export class ConversationAvatarComponent {
  @Input()
  contact: ChatContactDto;

  @Input()
  small: boolean | string;

  get avatarText(): string {
    if (!this.contact || !this.contact.username) return;
    if (this.contact.name && this.contact.surname)
      return this.contact.name[0] + this.contact.surname[0];
    if (this.contact.name) return this.contact.name.slice(0, 2);
    return this.contact.username.slice(0, 2);
  }

  get avatarBgStyle() {
    if (!this.contact || !this.contact.username) return;

    let hash = 0;
    for (let i = 0; i < this.contact.username.length; i++) {
      hash = this.contact.username.charCodeAt(i) + ((hash << 5) - hash);
    }
    return avatarStyles[Math.abs(hash % 10)];
  }

  get isSmall() {
    return typeof this.small === 'string' || this.small;
  }
}
