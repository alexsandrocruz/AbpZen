import { NgModule } from '@angular/core';
import { BrandLogoComponent } from './brand-logo.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [BrandLogoComponent],
  imports: [
    RouterModule
  ],
  exports: [BrandLogoComponent]
})
export class LpxBrandLogoModule { }
