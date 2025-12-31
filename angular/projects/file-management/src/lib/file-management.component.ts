import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';
import { eFileManagementComponents } from './enums';
import { DeleteService } from './services/delete.service';
import { DirectoryTreeService } from './services/directory-tree.service';
import { DownloadService } from './services/download.service';
import { MoveService } from './services/move.service';
import { NavigatorService } from './services/navigator.service';
import { UpdateStreamService } from './services/update-stream.service';
import { UploadService } from './services/upload.service';

@Component({
  selector: 'abp-file-management',
  templateUrl: './file-management.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    DeleteService,
    DownloadService,
    DirectoryTreeService,
    NavigatorService,
    MoveService,
    UploadService,
    UpdateStreamService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eFileManagementComponents.FolderContent,
    },
  ],
})
export class FileManagementComponent implements OnInit {
  currentContent: DirectoryContentDto[] = [];

  constructor(private directory: DirectoryTreeService) {}

  ngOnInit(): void {
    this.directory.updateDirectories(null);
  }
}
