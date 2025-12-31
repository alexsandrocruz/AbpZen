import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NgxValidateCoreModule,
  VALIDATION_ERROR_TEMPLATE,
  VALIDATION_INVALID_CLASSES,
  VALIDATION_TARGET_SELECTOR,
} from '@ngx-validate/core';
import { CoreModule } from '@abp/ng.core';
import { AbpValidationErrorComponent } from './abp-validation-error.component';

@NgModule({
  declarations: [AbpValidationErrorComponent],
  imports: [CommonModule, CoreModule, NgxValidateCoreModule],
  exports: [AbpValidationErrorComponent, NgxValidateCoreModule],
})
export class AbpValidationErrorModule {
  static forRoot(): ModuleWithProviders<AbpValidationErrorModule> {
    return {
      ngModule: AbpValidationErrorModule,
      providers: [
        {
          provide: VALIDATION_ERROR_TEMPLATE,
          useValue: AbpValidationErrorComponent,
        },
        {
          provide: VALIDATION_INVALID_CLASSES,
          useValue: 'is-invalid',
        },
        {
          provide: VALIDATION_TARGET_SELECTOR,
          useValue: '.form-group',
        },
      ],
    };
  }
}
