import { ListService } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ePaymentRouteNames } from '@volo/abp.ng.payment/admin/config';
import { PaymentRequestAdminService, PaymentRequestProductDto } from '@volo/abp.ng.payment/proxy';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ePaymentComponents } from '../../enums/components';

@Component({
  selector: 'abp-products',
  templateUrl: './products.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: ePaymentComponents.Products,
    },
  ],
})
export class ProductsComponent {
  data: PaymentRequestProductDto[] = [];

  breadCrumbItems = [
    {
      name: ePaymentRouteNames.Payment,
    },
    {
      name: ePaymentRouteNames.Requests,
      path: '/payment/requests',
    },
    {
      name: this.requestId,
    },
    {
      name: 'Payment::Products',
    },
  ];

  get requestId() {
    return this.route.snapshot.params.requestId;
  }

  constructor(
    private route: ActivatedRoute,
    private service: PaymentRequestAdminService,
    public list: ListService,
  ) {
    (
      list.hookToQuery(
        () => this.service.get(this.requestId).pipe(map(request => request.products)) as any,
      ) as unknown as Observable<PaymentRequestProductDto[]>
    ).subscribe((data: PaymentRequestProductDto[]) => (this.data = data));
  }
}
