import {
  Component,
  DestroyRef,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild,
  inject,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { filter, finalize, of, switchMap, take, tap } from 'rxjs';
import { CoreModule } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { ImportUsersFromFileOutput, ImportUsersFromFileType } from '@volo/abp.ng.identity/proxy';
import { ExcelService } from '../../services/excel.service';

@Component({
  standalone: true,
  selector: 'abp-excel-upload-file',
  templateUrl: './upload-excel-file.component.html',
  imports: [CoreModule, ThemeSharedModule],
})
export class UploadExcelFileComponent {
  protected readonly fb = inject(FormBuilder);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly excelService = inject(ExcelService);

  @Input() fileType: ImportUsersFromFileType;
  @Output() isModalVisibleChange = new EventEmitter<boolean>();
  @Output() isListChange = new EventEmitter<boolean>();
  @ViewChild('fileInput', { static: false }) fileInput: ElementRef;
  isModalVisible = true;
  form = this.fb.group({
    file: [null],
  });
  selectedFile: File;
  loading = false;

  get fileName() {
    return this.selectedFile?.name;
  }

  get acceptFileType() {
    return this.excelService.getFileType(this.fileType);
  }

  changeFile(event) {
    if (event.target.files.length) {
      this.selectedFile = event.target.files[0];
    }

    if (this.selectedFile) {
      this.fileInput.nativeElement.value = '';
    }
  }

  downloadSample() {
    this.excelService.downloadSample(this.fileType);
  }

  resetList() {
    this.isModalVisibleChange.emit(false);
    this.isListChange.emit();
  }

  onSubmit() {
    this.loading = true;
    let response: ImportUsersFromFileOutput | null = null;

    const warningConfirmation = () => {
      return this.confirmationService
        .warn('AbpIdentity::ImportFailedMessage', 'AbpUi::Warning', {
          yesText: 'AbpUi::Yes',
          messageLocalizationParams: [
            response.succeededCount.toString(),
            response.failedCount.toString(),
          ],
        })
        .pipe(
          take(1),
          filter(res => res === Confirmation.Status.confirm),
          switchMap(() => {
            this.excelService.downloadInvalidUsers(
              this.fileType,
              response.invalidUsersDownloadToken,
            );
            return of(null);
          }),
          finalize(() => {
            this.loading = false;
            if (response.succeededCount) {
              this.isListChange.emit();
            }
          }),
        )
        .subscribe();
    };

    const succesfullConfirmation = () => {
      return this.confirmationService
        .success('AbpIdentity::ImportSuccessMessage', 'AbpUi::Success', {
          hideCancelBtn: true,
          yesText: 'AbpUi::Ok',
        })
        .pipe(
          take(1),
          finalize(() => {
            this.resetList();
          }),
        )
        .subscribe();
    };

    if (!this.selectedFile) {
      this.confirmationService
        .warn('AbpIdentity::PleaseSelectFile', 'AbpUi::Warning', {
          hideCancelBtn: true,
          yesText: 'AbpUi::Ok',
        })
        .pipe(
          take(1),
          finalize(() => (this.loading = false)),
        )
        .subscribe();
      return;
    }

    const myFormData = new FormData();
    myFormData.append('file', this.selectedFile);
    myFormData.append('fileType', this.fileType.toString());

    this.excelService
      .uploadFile(myFormData)
      .pipe(
        tap(res => (response = res)),
        finalize(() => {
          this.loading = false;
          if (response.isAllSucceeded) {
            return succesfullConfirmation();
          }

          return warningConfirmation();
        }),
      )
      .subscribe();
  }
}
