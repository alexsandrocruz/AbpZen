import { Injectable, inject } from '@angular/core';
import { map, switchMap } from 'rxjs';
import { AbpWindowService } from '@abp/ng.core';
import { IdentityUserService, ImportUsersFromFileType } from '@volo/abp.ng.identity/proxy';

@Injectable()
export class ExcelService {
  protected readonly identityUserService = inject(IdentityUserService);
  protected readonly abpWindowService = inject(AbpWindowService);

  fileTypes: Record<ImportUsersFromFileType, string> = {
    [ImportUsersFromFileType.Excel]: '.xlsx',
    [ImportUsersFromFileType.Csv]: '.csv',
  };

  getFileType(fileType: ImportUsersFromFileType): string {
    return this.fileTypes[fileType];
  }

  protected getDownloadToken() {
    return this.identityUserService.getDownloadToken().pipe(map(val => val.token));
  }

  exportExcel() {
    this.getDownloadToken()
      .pipe(
        switchMap(token => {
          return this.identityUserService.getListAsExcelFile({
            token: token,
            maxResultCount: undefined,
          });
        }),
      )
      .subscribe(blob => {
        this.abpWindowService.downloadBlob(
          blob,
          'UserList' + this.getFileType(ImportUsersFromFileType.Excel),
        );
      });
  }

  exportCSV() {
    this.getDownloadToken()
      .pipe(
        switchMap(token => {
          return this.identityUserService.getListAsCsvFile({
            token: token,
            maxResultCount: undefined,
          });
        }),
      )
      .subscribe(blob => {
        this.abpWindowService.downloadBlob(
          blob,
          'UserList' + this.getFileType(ImportUsersFromFileType.Csv),
        );
      });
  }

  downloadSample(fileType: ImportUsersFromFileType) {
    this.getDownloadToken()
      .pipe(
        switchMap(token => {
          return this.identityUserService.getImportUsersSampleFile({
            fileType: fileType,
            token: token,
          });
        }),
      )
      .subscribe(blob => {
        this.abpWindowService.downloadBlob(
          blob,
          'ImportUsersSampleFile' + this.getFileType(fileType),
        );
      });
  }

  uploadFile(input: FormData) {
    return this.identityUserService.importUsersFromFile(input);
  }

  downloadInvalidUsers(fileType: ImportUsersFromFileType, token: string) {
    this.identityUserService.getImportInvalidUsersFile({ token: token }).subscribe(blob => {
      this.abpWindowService.downloadBlob(blob, 'InvalidUsers' + this.getFileType(fileType));
    });
  }
}
