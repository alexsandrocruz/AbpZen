import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import {
  LpxTranslateModule,
  LpxVisibleDirective,
  ToObservableModule,
} from '@volo/ngx-lepton-x.core';
import { ToolbarItem } from '../models';

@Component({
  standalone: true,
  selector: 'lpx-mobile-toolbar-item',
  templateUrl: './mobile-toolbar-item.component.html',
  imports: [
    AsyncPipe,
    ToObservableModule,
    LpxTranslateModule,
    LpxVisibleDirective,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MobileToolbarItemComponent {
  @Input({ required: true }) item!: ToolbarItem;

  get count(): number {
    const { count } = this.item?.badge || {};
    return <number>count || 0;
  }

  get color(): string | undefined {
    return this.item.badge?.color;
  }

  get icon(): string | undefined {
    return this.item.icon;
  }

  get badgeIcon(): string | undefined {
    return this.item.badge?.icon;
  }

  action() {
    const { item } = this;

    if (!item.action) {
      return void 0;
    }

    return item.action();
  }
}
