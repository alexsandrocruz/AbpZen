import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RequestPersonalDataService } from '../../services/request-personal-data.service';

@Component({
  selector: 'abp-request-personal-data-toolbar-action',
  templateUrl: './request-personal-data-toolbar-action.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RequestPersonalDataToolbarActionComponent {
  constructor(public requestPersonalDataService: RequestPersonalDataService) {}
}
