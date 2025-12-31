import { Component, EventEmitter, Injector, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import { LocalizationModule, generatePassword } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService, getPasswordValidators } from '@abp/ng.theme.shared';
import { IdentityUserDto, IdentityUserService } from '@volo/abp.ng.identity/proxy';

@Component({
  standalone: true,
  selector: 'abp-set-user-password',
  templateUrl: './set-password-modal.component.html',
  imports: [ThemeSharedModule, LocalizationModule, ReactiveFormsModule],
})
export class SetUserPasswordComponent implements OnInit {
  protected readonly fb = inject(FormBuilder);
  protected readonly injector = inject(Injector);
  protected readonly service = inject(IdentityUserService);
  protected readonly toasterService = inject(ToasterService);

  @Input() selected: IdentityUserDto;
  @Output() visibleChange = new EventEmitter<boolean>();
  isModalVisible = true;
  form: UntypedFormGroup;

  ngOnInit(): void {
    this.form = this.fb.group({
      newPassword: [
        '',
        {
          validators: [Validators.required, ...getPasswordValidators(this.injector)],
        },
      ],
    });
  }

  generatePassword() {
    this.form.markAsDirty();
    const generatedPassword = generatePassword(this.injector);
    this.form.controls.newPassword.setValue(generatedPassword);
  }

  setPassword() {
    if (this.form.invalid) {
      return;
    }

    this.service.updatePassword(this.selected.id, this.form.value).subscribe(() => {
      this.visibleChange.emit(false);
      this.selected = {} as IdentityUserDto;
      this.form.reset();
      this.toasterService.success('AbpIdentity::SavedSuccessfully');
    });
  }
}
