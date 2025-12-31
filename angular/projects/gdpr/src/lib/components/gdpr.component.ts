import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ConfigStateService, ListService, PagedResultDto } from '@abp/ng.core';
import { GdprRequestDto, GdprRequestService } from '@volo/abp.ng.gdpr/proxy';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { eGdprComponents } from '../enums';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { filter } from 'rxjs/operators';
import { RequestPersonalDataService } from '../services/request-personal-data.service';

@Component({
  selector: 'abp-gdpr',
  templateUrl: './gdpr.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eGdprComponents.PersonalData,
    },
    RequestPersonalDataService,
  ],
})
export class GdprComponent implements OnInit {
  data: PagedResultDto<GdprRequestDto> = { items: [], totalCount: 0 };

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id');
  }

  constructor(
    public readonly list: ListService,
    protected service: GdprRequestService,
    private configState: ConfigStateService,
    private cdr: ChangeDetectorRef,
    private confirmation: ConfirmationService,
    private requestPersonalDataService: RequestPersonalDataService,
  ) {}

  ngOnInit() {
    this.hookToQuery();
    this.requestPersonalDataService.checkRequestAllowed();
  }

  deleteData() {
    this.confirmation
      .warn('AbpGdpr::DeletePersonalDataWarning', '')
      .pipe(filter(status => status === Confirmation.Status.confirm))
      .subscribe(() => this.deleteUserData());
  }

  deleteUserData() {
    this.service.deleteUserData().subscribe(() => {
      this.list.get();
      this.confirmation.success('AbpGdpr::PersonalDataDeleteRequestReceived', '');
    });
  }

  private hookToQuery() {
    const userId = this.currentUserId;
    this.list
      .hookToQuery(query => this.service.getList({ ...query, userId }))
      .subscribe(res => {
        this.data = res;
        this.cdr.detectChanges();
      });
  }
}
