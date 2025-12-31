import { Component, inject } from '@angular/core';
import {
  IdentityUserDelegationService,
  UserDelegationDto,
} from '@volo/abp.ng.account/public/proxy';
import { ListService, SubscriptionService } from '@abp/ng.core';
import { of } from 'rxjs';
import { AbpAuthorityDelegationService } from '../../services';

@Component({
  selector: 'abp-my-delegated-users',
  templateUrl: './my-delegated-users.component.html',
  styles: [],
  providers: [ListService],
})
export class MyDelegatedUsersComponent {
  private readonly subscription = inject(SubscriptionService);
  private readonly identityUserDelegationService = inject(IdentityUserDelegationService);
  public readonly service = inject(AbpAuthorityDelegationService);
  public readonly list = inject(ListService);

  filterText = '';
  filteredData: UserDelegationDto[];
  resultCount: number;

  data: UserDelegationDto[];
  get pageStartIndex() {
    return this.list.page * this.list.maxResultCount;
  }

  get pageEndIndex() {
    return this.pageStartIndex + this.list.maxResultCount;
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

  getAllList() {
    this.identityUserDelegationService.getMyDelegatedUsers().subscribe(res => {
      this.data = res.items;
      this.filteredData = this.getPageData(this.pageStartIndex, this.pageEndIndex);
    });
    this.list
      .hookToQuery(query =>
        of({ items: this.getPageData(query.skipCount, query.skipCount + query.maxResultCount) }),
      )
      .subscribe(res => (this.filteredData = res.items));
  }

  ngOnInit(): void {
    this.list.maxResultCount = 5;
    this.getAllList();
  }

  login(row: UserDelegationDto): void {
    const result$ = this.service.delegatedImpersonate(row.id);
    this.subscription.addOne(result$, () => {
      this.service.modalRef.instance.isModalOpen = false;
    });
  }
}
