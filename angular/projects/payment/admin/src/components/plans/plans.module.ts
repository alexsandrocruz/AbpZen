import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AdvancedEntityFiltersModule } from '@volo/abp.commercial.ng.ui';
import { PaymentPlansComponent } from './plans.component';

@NgModule({
  imports: [
    CommonModule,
    PageModule,
    CoreModule,
    ExtensibleModule,
    ThemeSharedModule,
    ReactiveFormsModule,
    NgxValidateCoreModule,
    AdvancedEntityFiltersModule,
  ],
  exports: [],
  declarations: [PaymentPlansComponent],
  providers: [],
})
export class PaymentPlansModule {}
