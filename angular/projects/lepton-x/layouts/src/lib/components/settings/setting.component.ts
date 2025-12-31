import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Setting } from './setting.model';

@Component({
  selector: 'lpx-setting',
  template: `
    <div class="setting-icon" (click)="clickOnSetting(setting)">
      @if (setting.icon) {
        <lpx-icon class="setting" [iconClass]="setting.icon"></lpx-icon>
      } @else {
        <span>{{ setting.shortText }}</span>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingComponent {
  @Input() setting!: Setting;

  @Output()
  settingClick = new EventEmitter<Setting>();

  clickOnSetting(setting: Setting): void {
    this.settingClick.emit(setting);
  }
}
