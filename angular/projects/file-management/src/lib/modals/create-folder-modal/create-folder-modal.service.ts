import { Injectable } from '@angular/core';
import { DirectoryDescriptorService } from '@volo/abp.ng.file-management/proxy';
import { tap } from 'rxjs/operators';
import { DirectoryTreeService, mapRootIdToEmpty } from '../../services/directory-tree.service';
import { UpdateStreamService } from '../../services/update-stream.service';

@Injectable()
export class CreateFolderModalService {
  constructor(
    private service: DirectoryDescriptorService,
    private updateStream: UpdateStreamService,
    private directoryService: DirectoryTreeService
  ) {}

  create(name: string, parent?: string) {
    const parentId = mapRootIdToEmpty(parent || this.updateStream.currentDirectory);
    return this.service
      .create({ name, parentId: mapRootIdToEmpty(parentId), extraProperties: {} })
      .pipe(
        tap(_ => {
          this.directoryService.updateDirectories(parentId);
          this.updateStream.patchStore({createdDirectory:parentId});
          this.directoryService.refreshNode({ newParentId: parentId });
        })
      );
  }
}
