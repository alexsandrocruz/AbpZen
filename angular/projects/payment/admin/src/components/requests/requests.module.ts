import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RequestsComponent } from './requests.component';
import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ReactiveFormsModule } from '@angular/forms';
import { AdvancedEntityFiltersModule } from '@volo/abp.commercial.ng.ui';
import { RequestsSearchFormComponent } from './requests-search-form/requests-search-form.component';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { SubscriptionTooltipModule } from '../subscription-tooltip/subscription-tooltip.module';

@NgModule({
  declarations: [RequestsComponent, RequestsSearchFormComponent],
  imports: [
    CommonModule,
    PageModule,
    CoreModule,
    ExtensibleModule,
    ThemeSharedModule,
    ReactiveFormsModule,
    AdvancedEntityFiltersModule,
    NgbDatepickerModule,
    SubscriptionTooltipModule,
  ],
})
export class RequestsModule {}
