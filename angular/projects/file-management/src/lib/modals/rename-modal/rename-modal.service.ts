import { Injectable } from '@angular/core';
import {
  DirectoryContentDto,
  DirectoryDescriptorDto,
  DirectoryDescriptorService,
  FileDescriptorDto,
  FileDescriptorService,
} from '@volo/abp.ng.file-management/proxy';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { UpdateStreamService } from '../../services/update-stream.service';

@Injectable()
export class RenameModalService {
  constructor(
    private directoryDescriptorService: DirectoryDescriptorService,
    private fileService: FileDescriptorService,
    private updateStream: UpdateStreamService,
  ) {}

  rename(contentToRename: DirectoryContentDto) {
    const id = contentToRename.id;
    const name = contentToRename.name;
    if (contentToRename.isDirectory) {
      return this.callService(this.directoryDescriptorService, id, name).pipe(
        tap(_ => {
          this.updateStream.patchStore({ renamedDirectory: { id, name } });
        }),
      );
    } else {
      return this.callService(this.fileService, id, name).pipe(
        tap(_ => this.updateStream.refreshContent()),
      );
    }
  }

  private callService(
    service: FileDescriptorService | DirectoryDescriptorService,
    id: string,
    name: string,
  ) {
    return (
      service.rename(id, { name }) as Observable<FileDescriptorDto | DirectoryDescriptorDto>
    ).pipe(map(() => true));
  }
}
