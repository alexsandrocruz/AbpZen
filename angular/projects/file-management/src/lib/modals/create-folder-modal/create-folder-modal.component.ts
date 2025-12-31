import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { CreateFolderModalService } from './create-folder-modal.service';
import { BaseModalComponent } from '../base-modal.component';

@Component({
  selector: 'abp-create-folder-modal',
  templateUrl: './create-folder-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CreateFolderModalService],
})
export class CreateFolderModalComponent extends BaseModalComponent {
  form = this.fb.group({
    name: ['', Validators.required],
  });

  @Input() parentId: string;

  constructor(
    private fb: UntypedFormBuilder,
    private service: CreateFolderModalService
  ) {
    super();
  }

  shouldSave() {
    return this.form.valid;
  }

  saveAction() {
    return this.service.create(this.form.controls.name.value, this.parentId);
  }

  clear() {
    this.form.reset();
  }
}
