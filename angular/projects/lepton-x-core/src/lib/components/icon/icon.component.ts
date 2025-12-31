import { Component, Inject, Input, ViewEncapsulation } from '@angular/core';
import { LEPTON_X_ICON_SET, ICON_MAP_TYPE } from './icon.token';

@Component({
  selector: 'lpx-icon',
  template: ` <i class="lpx-icon" [ngClass]="styleClass" aria-hidden="true"></i> `,
  encapsulation: ViewEncapsulation.None,
})
export class IconComponent {
  @Input() iconClass?: keyof ICON_MAP_TYPE | string;

  get styleClass() {
    return this.iconSet[this.iconClass as keyof ICON_MAP_TYPE] || this.iconClass;
  }

  constructor(@Inject(LEPTON_X_ICON_SET) public iconSet: ICON_MAP_TYPE) {}
}
