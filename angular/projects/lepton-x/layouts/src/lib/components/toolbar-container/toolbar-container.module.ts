import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  LPX_TRANSLATE_TOKEN,
  LpxAvatarModule,
  LpxClickOutsideModule,
  LpxNavbarModule,
  LpxTranslateModule,
} from '@volo/ngx-lepton-x.core';
import { LpxContextMenuModule } from '@volosoft/ngx-lepton-x';
import { LpxToolbarModule } from '../toolbar';
import { ToolbarContainerComponent } from './toolbar-container.component';
import { ToolbarTranslateDefaults } from './enums';

@NgModule({
  declarations: [ToolbarContainerComponent],
  imports: [
    CommonModule,
    LpxContextMenuModule,
    LpxClickOutsideModule,
    LpxAvatarModule,
    LpxNavbarModule,
    LpxToolbarModule,
    LpxTranslateModule,
  ],
  exports: [ToolbarContainerComponent],
})
export class LpxToolbarContainerModule {
  static forRoot(): ModuleWithProviders<LpxToolbarContainerModule> {
    return {
      ngModule: LpxToolbarContainerModule,
      providers: [
        {
          provide: LPX_TRANSLATE_TOKEN,
          useValue: [ToolbarTranslateDefaults],
          multi: true,
        },
      ],
    };
  }
}
