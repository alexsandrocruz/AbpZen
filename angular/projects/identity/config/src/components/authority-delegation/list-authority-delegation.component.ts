import { Component, inject, OnInit } from '@angular/core';
import { combineLatest, of, switchMap } from 'rxjs';
import { filter, tap } from 'rxjs/operators';
import { ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import {
  IdentityUserDelegationService,
  UserDelegationDto,
} from '@volo/abp.ng.account/public/proxy';
import { AbpAuthorityDelegationService } from '../../services';

@Component({
  selector: 'abp-list-authority-delegation',
  templateUrl: './list-authority-delegation.component.html',
  providers: [ListService],
})
export class ListAuthorityDelegationComponent implements OnInit {
  public readonly identityUserDelegationService = inject(IdentityUserDelegationService);
  private readonly confirmation = inject(ConfirmationService);
  public readonly service = inject(AbpAuthorityDelegationService);
  public readonly list = inject(ListService);

  filterText = '';
  filteredData: UserDelegationDto[];
  resultCount: number;
  isNewUserDelegateModalOpen = false;
  data: UserDelegationDto[];

  get pageStartIndex() {
    return this.list.page * this.list.maxResultCount;
  }

  get pageEndIndex() {
    return this.pageStartIndex + this.list.maxResultCount;
  }

  ngOnInit(): void {
    this.list.maxResultCount = 5;
    this.getAllList();
  }

  getAllList() {
    combineLatest([this.getDeletagedUser(), this.hookToQuery()]).subscribe();
  }

  getDeletagedUser() {
    return this.identityUserDelegationService.getDelegatedUsers().pipe(
      tap(({ items } = {} as any) => {
        this.data = items;
        this.filteredData = this.getPageData(this.pageStartIndex, this.pageEndIndex);
      }),
    );
  }

  hookToQuery() {
    return this.list
      .hookToQuery(query =>
        of({ items: this.getPageData(query.skipCount, query.skipCount + query.maxResultCount) }),
      )
      .pipe(tap(({ items } = {} as any) => (this.filteredData = items)));
  }

  filterByText() {
    this.filteredData = this.getPageData(this.pageStartIndex, this.pageEndIndex);
  }

  getFilteredData() {
    const list = this.data.filter(user =>
      user.userName.toLowerCase().includes(this.filterText.toLowerCase()),
    );
    this.resultCount = list.length;
    return list;
  }

  getPageData(start: number, end: number) {
    return this.getFilteredData().slice(start, end);
  }

  create() {
    this.isNewUserDelegateModalOpen = true;
  }

  delete(row: UserDelegationDto) {
    const id = row.id;
    this.confirmation
      .warn(`AbpAccount::DeleteUserDelegationConfirmationMessage`, 'AbpAccount::AreYouSure', {
        messageLocalizationParams: [row.userName],
      })
      .pipe(
        filter(res => res === Confirmation.Status.confirm),
        switchMap(() => this.identityUserDelegationService.deleteDelegation(id)),
        tap(() => this.getAllList()),
      )
      .subscribe();
  }

  userDelegationVisibleChange(visible: boolean = false) {
    this.isNewUserDelegateModalOpen = visible;
  }
}
