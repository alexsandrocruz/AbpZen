import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  LpxAvatarModule,
  LpxClickOutsideModule,
  LpxIconModule,
  LpxNavbarModule,
  LpxVisibleDirective,
} from '@volo/ngx-lepton-x.core';
import { ToolbarComponent } from './toolbar.component';
import { ToolbarItemComponent } from './toolbar-item/toolbar-item.component';
import { ToolbarItemsComponent } from './toolbar-items/toolbar-items.component';
import { ToolbarItemsDirective } from './toolbar-items/toolbar-items.directive';

const exportedDeclarations = [
  ToolbarComponent,
  ToolbarItemsComponent,
  ToolbarItemsDirective,
];

@NgModule({
  declarations: [...exportedDeclarations, ToolbarItemComponent],
  imports: [
    CommonModule,
    LpxIconModule,
    LpxAvatarModule,
    LpxClickOutsideModule,
    LpxNavbarModule,
    LpxVisibleDirective
  ],
  exports: [...exportedDeclarations],
})
export class LpxToolbarModule { }
