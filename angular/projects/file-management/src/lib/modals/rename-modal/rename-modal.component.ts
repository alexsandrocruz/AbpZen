import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { tap } from 'rxjs';
import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';
import { BaseModalComponent } from '../base-modal.component';
import { RenameModalService } from './rename-modal.service';

@Component({
  selector: 'abp-rename-modal',
  templateUrl: './rename-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [RenameModalService],
})
export class RenameModalComponent extends BaseModalComponent {
  @Output() contentRenamed = new EventEmitter();
  @Output() contentToRenameChange = new EventEmitter<DirectoryContentDto>();

  form = this.fb.group({
    name: ['', Validators.required],
  });

  // tslint:disable-next-line: variable-name
  _contentToRename: DirectoryContentDto;
  @Input()
  set contentToRename(val: DirectoryContentDto) {
    this._contentToRename = val;
    this.form.controls.name.patchValue(val?.name);
  }

  get contentToRename() {
    return this._contentToRename;
  }

  constructor(private service: RenameModalService, private fb: UntypedFormBuilder) {
    super();
  }

  saveAction() {
    return this.service
      .rename({
        ...this.contentToRename,
        ...this.form.value,
      })
      .pipe(
        tap(() => this.contentRenamed.emit(this.contentToRename))
      );
  }

  shouldSave() {
    return this.form.valid;
  }

  clear() {
    this.form.reset();
    this.contentToRename = null;
    this.contentToRenameChange.emit(this.contentToRename);
  }
}
