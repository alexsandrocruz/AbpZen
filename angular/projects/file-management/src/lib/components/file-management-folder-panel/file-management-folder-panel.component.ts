import { ListService } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Output,
} from '@angular/core';
import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';
import { NavigatorService } from '../../services/navigator.service';

@Component({
  selector: 'abp-file-management-folder-panel',
  templateUrl: './file-management-folder-panel.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ListService],
})
export class FileManagementFolderPanelComponent {
  @Output() contentUpdate = new EventEmitter<DirectoryContentDto[]>();

  constructor(public navigator: NavigatorService) {}
}
