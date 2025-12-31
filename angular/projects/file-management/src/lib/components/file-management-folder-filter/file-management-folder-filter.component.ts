import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ListService } from '@abp/ng.core';
import { NavigatorService } from '../../services/navigator.service';

@Component({
  selector: 'abp-file-management-folder-filter',
  templateUrl: './file-management-folder-filter.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileManagementFolderFilterComponent{
  constructor(
    public readonly list: ListService,
    private navigator: NavigatorService
  ) {}

  goUpFolder() {
    this.navigator.goUpFolder();
  }
}
