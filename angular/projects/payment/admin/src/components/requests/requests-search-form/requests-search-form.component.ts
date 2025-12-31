import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ListService } from '@abp/ng.core';
import {
  PaymentRequestGetListInput,
  paymentRequestStateOptions,
  paymentTypeOptions,
} from '@volo/abp.ng.payment/proxy';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { DateAdapter } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-payment-requests-search-form',
  templateUrl: './requests-search-form.component.html',
  providers: [{ provide: NgbDateAdapter, useClass: DateAdapter }],
})
export class RequestsSearchFormComponent {
  @Input() list: ListService;
  @Input() pageQuery: PaymentRequestGetListInput;
  @Output() pageQueryChange = new EventEmitter<PaymentRequestGetListInput>();

  paymentTypes = paymentTypeOptions;

  states = paymentRequestStateOptions;

  emit() {
    this.pageQueryChange.emit(this.pageQuery);
  }
}
