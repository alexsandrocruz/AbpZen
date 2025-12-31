import { Component, Inject } from '@angular/core';
import { ActionData, EXTENSIONS_ACTION_DATA } from '@abp/ng.components/extensible';
import { ImportUsersFromFileType } from '@volo/abp.ng.identity/proxy';
import { UsersComponent } from '../../users/users.component';

@Component({
  selector: 'abp-user-dropdown-menu',
  templateUrl: './user-dropdown-menu.component.html',
})
export class UserDropdownMenuComponent {
  component: UsersComponent;
  isExternalModalVisible = false;

  constructor(@Inject(EXTENSIONS_ACTION_DATA) public data: ActionData) {
    this.component = data.getInjected(UsersComponent);
  }

  onAddExternalLogin() {
    this.isExternalModalVisible = true;
  }

  getList() {
    this.component.list.get();
  }

  uploadExcelFile() {
    this.component.openUploadFileModal(ImportUsersFromFileType.Excel);
  }

  uploadCsvFile() {
    this.component.openUploadFileModal(ImportUsersFromFileType.Csv);
  }
}
