import { ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component, OnInit, inject } from '@angular/core';
import {
  GetIdentitySecurityLogListInput,
  IdentitySecurityLogDto,
  IdentitySecurityLogService,
} from '@volo/abp.ng.identity/proxy';
import { eIdentityComponents } from '../../enums/components';

@Component({
  selector: 'abp-security-logs',
  templateUrl: './security-logs.component.html',

  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.SecurityLogs,
    },
  ],
})
export class SecurityLogsComponent implements OnInit {
  protected readonly service = inject(IdentitySecurityLogService);
  public readonly list = inject(ListService);

  data: PagedResultDto<IdentitySecurityLogDto> = { items: [], totalCount: 0 };

  filter = {} as GetIdentitySecurityLogListInput;

  ngOnInit(): void {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query =>
        this.service.getList({
          ...query,
          ...this.filter,
        }),
      )
      .subscribe(res => (this.data = res));
  }

  clearFilters() {
    this.filter = {} as GetIdentitySecurityLogListInput;
  }
}
