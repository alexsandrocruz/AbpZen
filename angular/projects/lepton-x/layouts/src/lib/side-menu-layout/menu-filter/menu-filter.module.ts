import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LpxIconModule, LpxTranslateModule } from '@volo/ngx-lepton-x.core';

import { MenuFilterComponent } from './menu-filter.component';

@NgModule({
  declarations: [MenuFilterComponent],
  imports: [CommonModule, LpxIconModule, FormsModule, LpxTranslateModule],
  exports: [MenuFilterComponent],
})
export class MenuFilterModule {}
