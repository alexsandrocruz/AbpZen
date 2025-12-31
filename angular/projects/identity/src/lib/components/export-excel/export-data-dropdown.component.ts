import { Component, Inject } from '@angular/core';
import { ActionData, EXTENSIONS_ACTION_DATA } from '@abp/ng.components/extensible';
import { UsersComponent } from '../users/users.component';

@Component({
  selector: 'abp-export-data-dropdown',
  templateUrl: './export-data-dropdown.component.html',
})
export class ExportDataDropdownComponent {
  component: UsersComponent;
  constructor(@Inject(EXTENSIONS_ACTION_DATA) public data: ActionData) {
    this.component = data.getInjected(UsersComponent);
  }

  exportExcel() {
    this.component.exportExcel();
  }

  exportCSV() {
    this.component.exportCSV();
  }
}
