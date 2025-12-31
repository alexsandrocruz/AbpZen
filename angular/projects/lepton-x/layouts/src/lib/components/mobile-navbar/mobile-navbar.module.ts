import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {
  LPX_TRANSLATE_TOKEN,
  LpxAvatarModule,
  LpxBrandLogoModule,
  LpxIconModule,
  LpxNavbarModule,
  LpxTranslateModule,
  ToObservableModule,
} from '@volo/ngx-lepton-x.core';

import { MobileMenuTranslateDefaults } from './enums/mobile-menu-translate.enum';
import { MobileNavbarComponent } from './mobile-navbar.component';
import { MobileToolbarComponent } from '../toolbar/mobile-toolbar/mobile-toolbar.component';

@NgModule({
  declarations: [MobileNavbarComponent],
  imports: [
    CommonModule,
    RouterModule,
    LpxAvatarModule,
    LpxBrandLogoModule,
    LpxIconModule,
    LpxNavbarModule,
    LpxTranslateModule,
    ToObservableModule,
    MobileToolbarComponent,
  ],
  exports: [MobileNavbarComponent],
})
export class LpxMobileNavbarModule {
  static forRoot(): ModuleWithProviders<LpxMobileNavbarModule> {
    return {
      ngModule: LpxMobileNavbarModule,
      providers: [
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [MobileMenuTranslateDefaults],
          multi: true,
        },
      ],
    };
  }
}
