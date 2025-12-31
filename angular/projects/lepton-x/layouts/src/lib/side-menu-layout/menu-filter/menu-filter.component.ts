import { Component, ViewEncapsulation } from '@angular/core';

import { MenuFilterService } from './menu-filter.service';
import { SideMenuLayoutTranslate } from '../enums';

@Component({
  selector: 'lpx-menu-filter',
  templateUrl: './menu-filter.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class MenuFilterComponent {
  menuFilterText = SideMenuLayoutTranslate.MenuFilter;
  constructor(public service: MenuFilterService) {}
}
