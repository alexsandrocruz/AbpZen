import { Component, ViewEncapsulation, Input } from '@angular/core';

export interface Avatar {
  type: 'icon' | 'image';
  source: string;
}

@Component({
  selector: 'lpx-avatar',
  templateUrl: './avatar.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AvatarComponent {
  @Input() avatar?: Avatar;
}
