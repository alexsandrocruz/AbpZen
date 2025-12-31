import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { SubscriptionTooltipComponent } from './subscription-tooltip.component';

@NgModule({
  declarations: [SubscriptionTooltipComponent],
  exports: [SubscriptionTooltipComponent],
  imports: [CommonModule, CoreModule, NgbTooltipModule],
})
export class SubscriptionTooltipModule {}
