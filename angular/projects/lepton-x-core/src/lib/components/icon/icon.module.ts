import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconComponent } from './icon.component';
import { LEPTON_X_ICON_SET, ICON_MAP_TYPE, ICON_MAP } from './icon.token';

export interface LpxIconSettings {
  iconSet?: ICON_MAP_TYPE;
}

@NgModule({
  declarations: [IconComponent],
  imports: [CommonModule],
  exports: [IconComponent],
})
export class LpxIconModule {
  static forRoot(options?: LpxIconSettings): ModuleWithProviders<LpxIconModule> {
    return {
      ngModule: LpxIconModule,
      providers: [
        {
          provide: LEPTON_X_ICON_SET,
          useValue: options?.iconSet || ICON_MAP,
        },
      ],
    };
  }
}
