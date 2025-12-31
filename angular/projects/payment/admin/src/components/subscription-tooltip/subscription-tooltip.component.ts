import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PROP_DATA_STREAM } from '@abp/ng.components/extensible';
import { SubscriptionTooltipData } from '../../models/subscription-tooltip-data';

@Component({
  selector: 'abp-subscription-tooltip',
  templateUrl: './subscription-tooltip.component.html',
})
export class SubscriptionTooltipComponent {
  constructor(@Inject(PROP_DATA_STREAM) public value$: Observable<SubscriptionTooltipData>) {}
}
