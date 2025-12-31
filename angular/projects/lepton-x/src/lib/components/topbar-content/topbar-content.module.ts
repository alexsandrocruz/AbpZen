import { NgModule } from '@angular/core';
import { TopbarContentComponent } from './topbar-content.component';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [TopbarContentComponent],
  exports: [TopbarContentComponent],
  imports: [CommonModule],
})
export class LpxTopbarContentModule {}
