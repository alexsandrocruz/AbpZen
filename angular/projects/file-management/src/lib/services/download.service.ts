import { EnvironmentService, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { FileDescriptorService } from '@volo/abp.ng.file-management/proxy';
import { tap } from 'rxjs/operators';
import { FileInfo } from '../models/common-types';

@Injectable()
export class DownloadService {
  apiName = 'FileManagement';

  get apiUrl() {
    return this.environment.getApiUrl(this.apiName);
  }

  constructor(
    private restService: RestService,
    private fileDescriptorService: FileDescriptorService,
    private environment: EnvironmentService
  ) {}

  downloadFile(file: FileInfo) {
    return this.fileDescriptorService.getDownloadToken(file.id).pipe(
      tap((res) => {
        window.open(
          `${this.apiUrl}/api/file-management/file-descriptor/download/${file.id}?token=${res.token}`,
          '_self'
        );
      })
    );
  }
}
