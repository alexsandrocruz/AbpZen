import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import {
  DirectoryContentDto,
  DirectoryDescriptorService,
  FileDescriptorService,
} from '@volo/abp.ng.file-management/proxy';
import { filter, switchMap, take, tap } from 'rxjs/operators';
import { UpdateStreamService } from './update-stream.service';

@Injectable()
export class DeleteService {
  private readonly confirmationService = inject(ConfirmationService);
  private readonly directoryService = inject(DirectoryDescriptorService);
  private readonly fileService = inject(FileDescriptorService);
  private readonly updateStream = inject(UpdateStreamService);
  private readonly toasterService = inject(ToasterService);

  deleteFolder(content: DirectoryContentDto) {
    return this.delete(
      'FileManagement::DirectoryDeleteConfirmationMessage',
      content,
      this.directoryService,
    ).pipe(tap(() => this.updateStream.patchStore({ deletedDirectory: content.id })));
  }

  deleteFile(content: DirectoryContentDto) {
    return this.delete(
      'FileManagement::FileDeleteConfirmationMessage',
      content,
      this.fileService,
    ).pipe(tap(() => this.updateStream.refreshContent()));
  }

  private delete(
    message: string,
    content: DirectoryContentDto,
    service: DirectoryDescriptorService | FileDescriptorService,
  ) {
    return this.confirmationService.warn(message, 'FileManagement::AreYouSure').pipe(
      filter(status => status === Confirmation.Status.confirm),
      take(1),
      switchMap(() => service.delete(content.id)),
      tap(() => {
        this.toasterService.success('AbpUi::DeletedSuccessfully');
      }),
    );
  }
}
