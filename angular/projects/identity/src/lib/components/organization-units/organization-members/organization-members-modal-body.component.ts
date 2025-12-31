import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { Component, Input, OnInit } from '@angular/core';
import {
  GetIdentityUsersInput,
  IdentityUserDto,
  IdentityUserService,
} from '@volo/abp.ng.identity/proxy';

@Component({
  selector: 'abp-organization-members-modal-body',
  template: `
    <div id="data-tables-table-filter" class="data-tables-filter mb-3">
      <div class="input-group">
        <input
          type="search"
          class="form-control"
          [placeholder]="'AbpUi::PagerSearch' | abpLocalization"
          [(ngModel)]="list.filter"
        />
      </div>
    </div>

    <div class="card">
      <abp-extensible-table
        actionsText=""
        [data]="allUnits.items"
        [recordsTotal]="allUnits.totalCount"
        [actionsColumnWidth]="38"
        [actionsTemplate]="customAction"
        [list]="list"
        (tableActivate)="onTableSelect($event)"
      ></abp-extensible-table>
    </div>

    <ng-template #customAction let-row>
      <input
        class="form-check-input"
        type="checkbox"
        [(ngModel)]="checkedUnits[row.id]"
        [disabled]="isCheckboxDisabled(row.id)"
      />
    </ng-template>
  `,
  providers: [ListService],
})
export class OrganizationMembersModalBodyComponent implements OnInit {
  @Input() checkedUnits: ABP.Dictionary<boolean>;
  @Input() isCheckboxDisabled: (id: string) => boolean;

  allUnits = { items: [] } as PagedResultDto<IdentityUserDto>;

  constructor(
    public readonly list: ListService<GetIdentityUsersInput>,
    private userService: IdentityUserService,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.userService.getList(query))
      .subscribe(response => {
        this.allUnits = response;
      });
  }
  onTableSelect(event) {
    if (event.type == 'click' && event.value != '' && !this.isCheckboxDisabled(event.row.id)) {
      this.checkedUnits[event.row.id] = !this.checkedUnits[event.row.id];
    }
  }
}
