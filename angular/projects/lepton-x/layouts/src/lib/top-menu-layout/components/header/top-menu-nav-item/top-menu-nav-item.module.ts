import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxTopMenuNavItemComponent } from './top-menu-nav-item.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [LpxTopMenuNavItemComponent],
  imports: [CommonModule, RouterModule],
  exports: [LpxTopMenuNavItemComponent],
})
export class TopMenuNavItemModule {}
