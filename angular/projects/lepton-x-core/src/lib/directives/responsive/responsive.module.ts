import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResponsiveDirective } from './responsive.directive';

@NgModule({
  declarations: [ResponsiveDirective],
  imports: [CommonModule],
  exports: [ResponsiveDirective],
})
export class LpxResponsiveModule {}
