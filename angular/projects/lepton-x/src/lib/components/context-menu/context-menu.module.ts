import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxBrandLogoModule } from '@volo/ngx-lepton-x.core';
import { ContextMenuHeaderComponent } from './components/context-menu-header.component';
import { ContextMenuActionGroupComponent } from './components/context-menu-action-group.component';
import { ContextMenuComponent } from './context-menu.component';
import { ContextMenuToggleDirective } from './context-menu-toggle.directive';

const declarationsWithExports = [
  ContextMenuComponent,
  ContextMenuHeaderComponent,
  ContextMenuActionGroupComponent,
  ContextMenuToggleDirective,
];

@NgModule({
  declarations: [...declarationsWithExports],
  exports: [...declarationsWithExports],
  imports: [CommonModule, LpxBrandLogoModule],
})
export class LpxContextMenuModule {}
