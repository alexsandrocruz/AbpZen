import { ModuleWithProviders, NgModule } from '@angular/core';

import { FooterComponent } from './footer.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [FooterComponent],
  exports: [FooterComponent],
  imports: [CommonModule, RouterModule],
})
export class LpxFooterModule {
  static forRoot(): ModuleWithProviders<LpxFooterModule> {
    return {
      ngModule: LpxFooterModule,
      providers: [],
    };
  }
}
