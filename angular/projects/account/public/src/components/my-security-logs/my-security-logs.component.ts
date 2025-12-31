import { ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { DateAdapter } from '@abp/ng.theme.shared';
import { Component, OnInit, inject } from '@angular/core';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AccountService, Volo } from '@volo/abp.ng.account/public/proxy';
import { eAccountComponents } from '../../enums/components';

@Component({
  selector: 'abp-my-security-logs',
  templateUrl: './my-security-logs.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eAccountComponents.MySecurityLogs,
    },
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
})
export class MySecurityLogsComponent implements OnInit {
  protected readonly service = inject(AccountService);
  public readonly list = inject(ListService);

  data: PagedResultDto<Volo.Abp.Identity.IdentitySecurityLogDto> = { items: [], totalCount: 0 };

  filter = {} as Partial<Volo.Abp.Identity.GetIdentitySecurityLogListInput>;

  ngOnInit(): void {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query =>
        this.service.getSecurityLogList({
          ...query,
          ...this.filter,
        }),
      )
      .subscribe(res => (this.data = res));
  }
}
