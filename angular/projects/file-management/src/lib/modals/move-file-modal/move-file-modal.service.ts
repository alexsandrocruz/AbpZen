import { InternalStore } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Injectable } from '@angular/core';
import {
  DirectoryContentDto,
  DirectoryDescriptorInfoDto,
  DirectoryDescriptorService,
  FileDescriptorService,
} from '@volo/abp.ng.file-management/proxy';
import { tap } from 'rxjs/operators';
import { FolderInfo } from '../../models/common-types';
import {
  mapRootIdToEmpty,
  ROOT_NODE,
} from '../../services/directory-tree.service';
import { MoveService } from '../../services/move.service';
import { NavigatorService } from '../../services/navigator.service';
import { UpdateStreamService } from '../../services/update-stream.service';

@Injectable()
export class MoveFileModalService {
  private directoryContentStore = new InternalStore(
    [] as DirectoryDescriptorInfoDto[]
  );
  directoryContent$ = this.directoryContentStore.sliceState((state) => state);

  private breadcrumbStore = new InternalStore([ROOT_NODE] as FolderInfo[]);
  breadcrumbs$ = this.breadcrumbStore.sliceState((state) => state);

  constructor(
    private service: DirectoryDescriptorService,
    private fileService: FileDescriptorService,
    private updateStream: UpdateStreamService,
    private toaster: ToasterService,
    private moveService: MoveService,
    private navigatorService: NavigatorService
  ) {}

  reset() {
    this.breadcrumbStore.reset();
    this.directoryContentStore.reset();
    this.updateContent().subscribe();
  }

  updateContent() {
    return this.service
      .getList(this.getCurrentFolderIdForRequest())
      .pipe(tap((content) => this.directoryContentStore.patch(content.items)));
  }

  goTo(folder: FolderInfo) {
    let navigatedFolders = this.getNavigatedFolders();
    const index = navigatedFolders.findIndex((f) => f.id === folder.id);
    if (index < 0) {
      navigatedFolders = [...navigatedFolders, folder];
    } else {
      navigatedFolders = navigatedFolders.slice(0, index + 1);
    }
    this.breadcrumbStore.patch(navigatedFolders);
    this.updateContent().subscribe();
  }

  move(fileToMove: DirectoryContentDto) {
    return this.fileService
      .move({
        id: fileToMove.id,
        newDirectoryId: this.getCurrentFolderIdForRequest(),
      })
      .pipe(
        tap((_) => {
          this.updateStream.refreshContent();
          this.toaster.success('FileManagement::SuccessfullyMoved');
        })
      );
  }

  moveFolder(folderToMove: DirectoryContentDto, oldParentId: string) {
    return this.moveService.moveTo(
      folderToMove,
      this.getCurrentFolder(),
      oldParentId
    );
  }

  private getCurrentFolder() {
    const navigatedFolders = this.getNavigatedFolders();
    return navigatedFolders[navigatedFolders.length - 1];
  }

  private getCurrentFolderIdForRequest() {
    const navigatedFolders = this.getNavigatedFolders();
    return mapRootIdToEmpty(navigatedFolders[navigatedFolders.length - 1]?.id);
  }

  private getNavigatedFolders(): FolderInfo[] {
    return this.breadcrumbStore.state;
  }
}
