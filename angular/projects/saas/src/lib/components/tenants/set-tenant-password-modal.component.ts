import { 
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { generatePassword, LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { TenantService } from '@volo/abp.ng.saas/proxy';
import { SetPasswordFormGroupModel } from '../../models/set-tenant-password-modal.model';

@Component({
  standalone: true,
  selector: 'abp-set-tenant-password-modal',
  templateUrl: 'set-tenant-password-modal.component.html',
  imports: [ReactiveFormsModule, NgbTooltipModule, LocalizationModule, ThemeSharedModule],
})
export class SetTenantPasswordModalComponent implements OnChanges {
  protected readonly fb = inject(FormBuilder);
  protected readonly tenantService = inject(TenantService);
  protected readonly toaster = inject(ToasterService);

  @Input()
  modalVisible = false;

  @Input()
  tenantName: string;

  @Input()
  tenantId = '';

  @Output()
  modalVisibleChange = new EventEmitter<void>();

  modalBusy: boolean;
  form: FormGroup<SetPasswordFormGroupModel>;
  fieldTextType = false;

  buildForm() {
    this.form = this.fb.group({
      adminName: ['admin', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  save(ngForm: FormGroupDirective) {
    if (this.form.invalid) {
      return;
    }

    const { adminName: username, password } = this.form.value;
    this.modalBusy = true;
    this.tenantService
      .setPassword(this.tenantId, { username, password })
      .subscribe(() => {
        this.toaster.success('AbpIdentity::PasswordChangedMessage');
        ngForm.resetForm();
        this.modalVisibleChange.emit();
      })
      .add(() => (this.modalBusy = false));
  }

  onModalVisibleChange(isVisible: boolean) {
    if (isVisible) {
      return;
    }
    this.modalVisibleChange.emit();
  }

  ngOnChanges({ modalVisible, tenantName }: SimpleChanges): void {
    if (modalVisible && modalVisible.currentValue) {
      this.buildForm();
    }
  }

  generatePassword() {
    const generatedPassword = generatePassword();
    this.form.controls.password.setValue(generatedPassword);
  }
}
