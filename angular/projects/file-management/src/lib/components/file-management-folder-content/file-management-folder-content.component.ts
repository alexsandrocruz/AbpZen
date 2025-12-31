import { ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import type {
  DirectoryContentDto,
  DirectoryContentRequestInput,
} from '@volo/abp.ng.file-management/proxy';
import { DirectoryDescriptorService } from '@volo/abp.ng.file-management/proxy';
import { merge, Subscription } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import { eFileManagementComponents } from '../../enums/components';
import { DeleteService } from '../../services/delete.service';
import { DownloadService } from '../../services/download.service';
import { NavigatorService } from '../../services/navigator.service';
import { UpdateStreamService } from '../../services/update-stream.service';

@Component({
  selector: 'abp-file-management-folder-content',
  templateUrl: './file-management-folder-content.component.html',
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eFileManagementComponents.FolderContent,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileManagementFolderContentComponent implements OnInit, OnDestroy {
  @Output() contentUpdate = new EventEmitter<DirectoryContentDto[]>();

  contentToRename: DirectoryContentDto;
  renameModalOpen = false;

  fileToMove: DirectoryContentDto;
  moveModalOpen = false;
  parentOfFileToMove: string;

  private updateContent = (data: PagedResultDto<DirectoryContentDto>) => {
    this.currentList = data.items;
    this.contentUpdate.emit(data.items);
  };

  currentList: DirectoryContentDto[];
  content$ = this.list
    .hookToQuery(query =>
      this.service.getContent({
        ...query,
        id: this.updateStream.currentDirectory,
      }),
    )
    .pipe(tap(this.updateContent));

  subscription: Subscription;

  constructor(
    public readonly list: ListService<DirectoryContentRequestInput>,
    public readonly service: DirectoryDescriptorService,
    private deleteService: DeleteService,
    private downloadService: DownloadService,
    private navigator: NavigatorService,
    private updateStream: UpdateStreamService,
  ) {}

  ngOnInit() {
    this.subscription = merge(
      this.updateStream.contentRefresh$,
      this.updateStream.directoryDelete$.pipe(filter(this.isDirectoryPresent)),
      this.updateStream.directoryRename$.pipe(
        map(directory => directory.id),
        filter(this.isDirectoryPresent),
      ),
    ).subscribe(this.list.get);
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  openFolder(record: DirectoryContentDto) {
    this.navigator.goToFolder({ id: record.id, name: record.name });
  }

  renameFolder(record: DirectoryContentDto) {
    this.openRenameModal(record);
  }

  deleteFolder(record: DirectoryContentDto) {
    this.deleteService.deleteFolder(record).subscribe();
  }

  downloadFile(record: DirectoryContentDto) {
    this.downloadService.downloadFile(record).subscribe();
  }

  renameFile(record: DirectoryContentDto) {
    this.openRenameModal(record);
  }

  deleteFile(record: DirectoryContentDto) {
    this.deleteService.deleteFile(record).subscribe();
  }

  moveFile(record: DirectoryContentDto) {
    this.fileToMove = record;
    this.moveModalOpen = true;
  }

  moveFolder(record: DirectoryContentDto) {
    this.fileToMove = record;
    this.parentOfFileToMove = this.navigator.getCurrentFolderId();
    this.moveModalOpen = true;
  }

  onContentSaved() {
    delete this.contentToRename;
    delete this.fileToMove;
    delete this.parentOfFileToMove;
  }

  private openRenameModal(withContent: DirectoryContentDto) {
    this.renameModalOpen = true;
    this.contentToRename = withContent;
  }

  private isDirectoryPresent = (id: string) => {
    return this.currentList.some(item => item.id === id);
  };
}
