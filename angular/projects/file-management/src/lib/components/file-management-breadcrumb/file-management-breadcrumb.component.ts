import {
  Component,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { FolderInfo } from '../../models/common-types';

@Component({
  selector: 'abp-file-management-breadcrumb',
  templateUrl: './file-management-breadcrumb.component.html',
  styleUrls: ['./file-management-breadcrumb.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileManagementBreadcrumbComponent {
  @Input() rootBreadcrumbName = 'FileManagement::AllFiles';
  @Input() navigatedFolders: FolderInfo[];
  @Output() breadcrumbClick = new EventEmitter<FolderInfo>();

  trackByIdAndName = (_, item: FolderInfo) => item.id + '-' + item.name;

  onClick(item: FolderInfo) {
    this.breadcrumbClick.emit(item);
  }
}
