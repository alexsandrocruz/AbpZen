import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'lpx-context-menu-action-group',
  template: ` <ng-content></ng-content> `,
})
export class ContextMenuActionGroupComponent {
  @HostBinding('class')
  public hostClass = 'lpx-context-menu--action-group';
}
