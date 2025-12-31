import {
  Component,
  EventEmitter,
  Injector,
  input,
  Input,
  Output,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { of } from 'rxjs';
import { take } from 'rxjs/operators';
import { getStream$ } from '../../../utils/common';
import { LpxNavbarItem } from '../models';
import { collapse } from '../../../animations/collapse.animations';

@Component({
  selector: 'lpx-sub-navbar',
  templateUrl: './sub-navbar.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [collapse],
})
export class SubNavbarComponent {
  protected readonly injector = inject(Injector);

  @Input() item!: LpxNavbarItem;

  routerItem = input<boolean>();

  @Output() routeClick = new EventEmitter<LpxNavbarItem>();
  @Output() expand = new EventEmitter<LpxNavbarItem>();

  onItemClick(menuItem: LpxNavbarItem): void {
    let action$ = of(true);
    if (menuItem.action) {
      const result = menuItem.action();
      action$ = getStream$(result);
    }
    action$.pipe(take(1)).subscribe((result) => {
      if (result) {
        this.processItemClick(menuItem);
      }
    });
  }

  onChildExpand(child: LpxNavbarItem): void {
    if (child.expanded) {
      this.item?.children
        ?.filter((otherChild) => otherChild !== child)
        .forEach((otherChild) => {
          otherChild.expanded = false;
          otherChild.selected = false;
        });
    }
  }

  processItemClick(menuItem: LpxNavbarItem): void {
    if (menuItem.children?.length) {
      menuItem.expanded = !menuItem.expanded;
      this.expand.emit(menuItem);
      return;
    }
    
    this.routeClick.emit(menuItem);
    
    if (!this.routerItem()) {
      menuItem.selected = true;
    }
  }
}
