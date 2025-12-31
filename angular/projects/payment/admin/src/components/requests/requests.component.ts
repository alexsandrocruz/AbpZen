import { ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  PaymentRequestAdminService,
  PaymentRequestGetListInput,
  PaymentRequestWithDetailsDto,
} from '@volo/abp.ng.payment/proxy';
import { Observable } from 'rxjs';
import { ePaymentComponents } from '../../enums/components';

@Component({
  selector: 'abp-payment-requests',
  templateUrl: './requests.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: ePaymentComponents.Requests,
    },
  ],
})
export class RequestsComponent {
  data$: Observable<PagedResultDto<PaymentRequestWithDetailsDto>>;
  pageQuery = {} as PaymentRequestGetListInput;
  constructor(
    public list: ListService,
    private paymentRequestAdminService: PaymentRequestAdminService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.data$ = this.list.hookToQuery(query =>
      this.paymentRequestAdminService.getList({ ...query, ...this.pageQuery }),
    );
  }
  goToProducts(id: string) {
    this.router.navigate([id, 'products'], { relativeTo: this.route });
  }
}
