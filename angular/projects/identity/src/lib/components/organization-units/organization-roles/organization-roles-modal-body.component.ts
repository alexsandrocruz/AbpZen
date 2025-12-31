import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { Component, Input, OnInit } from '@angular/core';
import {
  GetIdentityRoleListInput,
  IdentityRoleDto,
  IdentityRoleService,
} from '@volo/abp.ng.identity/proxy';

@Component({
  selector: 'abp-organization-roles-modal-body',
  template: `
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
export class OrganizationRolesModalBodyComponent implements OnInit {
  @Input() checkedUnits: ABP.Dictionary<boolean>;
  @Input() isCheckboxDisabled: (id: string) => boolean;

  allUnits = { items: [] } as PagedResultDto<IdentityRoleDto>;

  constructor(
    public readonly list: ListService<GetIdentityRoleListInput>,
    private roleService: IdentityRoleService,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.roleService.getList(query))
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
