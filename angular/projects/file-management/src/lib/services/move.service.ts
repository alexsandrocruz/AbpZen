import {
  Confirmation,
  ConfirmationService,
  ToasterService,
} from '@abp/ng.theme.shared';
import { Injectable } from '@angular/core';
import { DirectoryDescriptorService } from '@volo/abp.ng.file-management/proxy';
import { filter, switchMap, tap } from 'rxjs/operators';
import { FolderInfo } from '../models/common-types';
import { mapRootIdToEmpty } from './directory-tree.service';
import { UpdateStreamService } from './update-stream.service';

@Injectable()
export class MoveService {
  constructor(
    private service: DirectoryDescriptorService,
    private confirmation: ConfirmationService,
    private updateStream: UpdateStreamService,
    private toaster: ToasterService
  ) {}

  moveTo(source: FolderInfo, target: FolderInfo, oldParentId: string) {
    const id = source.id;
    const newParentId = mapRootIdToEmpty(target.id);
    return this.confirmation
      .warn(
        'FileManagement::DirectoryMoveConfirmMessage',
        'FileManagement::AreYouSure',
        { messageLocalizationParams: [source.name, target.name] }
      )
      .pipe(
        filter((status) => status === Confirmation.Status.confirm),
        switchMap((_) => this.service.move({ id, newParentId })),
        tap((_) => {
          this.updateStream.patchStore({
            movedDirectory: {
              id,
              newParentId,
              oldParentId,
            },
          });
          this.toaster.success('FileManagement::SuccessfullyMoved');
        })
      );
  }
}
