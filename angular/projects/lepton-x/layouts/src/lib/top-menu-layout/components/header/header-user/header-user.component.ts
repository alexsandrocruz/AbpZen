import {
  Component,
  ElementRef,
  HostBinding,
  Inject,
  ViewChild,
} from '@angular/core';
import { UserProfileService } from '@volo/ngx-lepton-x.core';
import { ContextMenuComponent } from '@volosoft/ngx-lepton-x';
import { ContextMenuTriggerEvent } from '../../../../models';
import { LPX_USER_MENU_EVENT_NAME } from '../../../../tokens';

@Component({
  selector: 'lpx-header-user',
  templateUrl: './header-user.component.html',
  styles: [],
})
export class HeaderUserComponent {
  @HostBinding('class') class = 'lpx-avatar-wrapper';
  @ViewChild('avatarContainer', { static: false })
  avatarContainerRef!: ElementRef;

  @ViewChild(ContextMenuComponent)
  ctxMenu!: ContextMenuComponent;

  constructor(
    public readonly userProfileService: UserProfileService,
    @Inject(LPX_USER_MENU_EVENT_NAME)
    private menuTriggerType: ContextMenuTriggerEvent
  ) {}

  openCtxMenu(eventType: ContextMenuTriggerEvent) {
    if (this.menuTriggerType !== eventType) {
      return;
    }
    this.ctxMenu.open();
  }

  closeCtxMenu(eventType: ContextMenuTriggerEvent) {
    if (this.menuTriggerType !== eventType) {
      return;
    }
    this.ctxMenu.close();
  }
}
