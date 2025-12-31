import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { ContextMenuComponent } from '@volosoft/ngx-lepton-x';
import { UserProfileService, ToolbarService } from '@volo/ngx-lepton-x.core';
import { ToolbarTranslateKeys } from './enums';

@Component({
  selector: 'lpx-toolbar-container',
  templateUrl: './toolbar-container.component.html',
})
export class ToolbarContainerComponent {
  protected readonly toolbarService = inject(ToolbarService);
  protected readonly userProfileService = inject(UserProfileService);

  @ViewChild(ContextMenuComponent, { static: false })
  ctxMenu!: ContextMenuComponent;

  profileRef$ = new ReplaySubject<ElementRef>(1);

  welcomeText = ToolbarTranslateKeys.ContextMenuWelcome;

  toggleCtxMenu(): void {
    this.ctxMenu.toggle();
  }
}
