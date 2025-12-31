import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule, LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import {
  ePropType,
  ExtensibleDateTimePickerComponent,
  FormProp,
} from '@abp/ng.components/extensible';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { UserLockService } from './user-lock.service';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';

@Component({
  selector: 'abp-user-lock-modal',
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    ThemeSharedModule,
    ExtensibleDateTimePickerComponent,
    LocalizationModule,
    ReactiveFormsModule,
  ],
  templateUrl: './user-lock-modal.component.html',
  providers: [UserLockService],
})
export class UserLockModalComponent {
  protected readonly service = inject(UserLockService);
  protected readonly toasterService = inject(ToasterService);
  form = this.service.buildLockForm();

  get busy() {
    return this.service.busy;
  }
  @Input({ required: true }) selected: IdentityUserDto;
  @Output() visibleChange = new EventEmitter<boolean>();

  dateTimePickerProps = FormProp.create({
    displayName: 'AbpIdentity::DisplayName:LockoutEnd',
    validators: () => [Validators.required],
    name: 'lockoutEnd',
    id: 'lockout-end',
    type: ePropType.DateTime,
  });

  submit() {
    if (this.form.invalid) {
      return;
    }
    this.service.lock(this.selected.id, this.form.value.lockoutEnd).subscribe(() => {
      this.toasterService.success('AbpUi::SavedSuccessfully');
      this.visibleChange.emit(false);
    });
  }
}
