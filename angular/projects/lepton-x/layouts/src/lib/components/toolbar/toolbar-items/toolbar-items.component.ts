import { Component, Input } from '@angular/core';
import { ToolbarItem } from '../models';

@Component({
  selector: 'lpx-toolbar-items',
  templateUrl: './toolbar-items.component.html',
})
export class ToolbarItemsComponent {
  @Input()
  items!: ToolbarItem[];
}
