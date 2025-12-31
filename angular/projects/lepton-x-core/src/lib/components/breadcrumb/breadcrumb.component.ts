import { Component, ViewEncapsulation } from '@angular/core';
import { BreadcrumbItem, BreadcrumbService } from './breadcrumb.service';
import { ICON_MAP } from '../icon/icon.token';

@Component({
  selector: 'lpx-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class BreadcrumbComponent {
  icon = ICON_MAP;

  constructor(public service: BreadcrumbService) {}

  onClick(item: BreadcrumbItem): void {
    if (item.children) {
      item.expanded = !item.expanded;
    }
  }

  // TODO: close dropdown when click outside
}
