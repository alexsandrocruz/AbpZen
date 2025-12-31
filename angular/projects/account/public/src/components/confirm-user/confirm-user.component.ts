import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ConfirmUserService } from '../../services';
import { PersonalSettingsVerifyButtonComponent } from '../personal-settings/personal-settings-verify-button/personal-settings-verify-button.component';

@Component({
  standalone: true,
  selector: 'abp-confirm-user',
  templateUrl: './confirm-user.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    ReactiveFormsModule,
    LocalizationModule,
    ThemeSharedModule,
    PersonalSettingsVerifyButtonComponent,
  ],
})
export class ConfirmUserComponent {
  protected readonly confirmUserService = inject(ConfirmUserService);
  protected readonly fb = inject(FormBuilder);

  email = this.confirmUserService.email;
  phone = this.confirmUserService.phone;

  phoneModalVisible = false;
  form = this.fb.group({ phoneNumber: ['', [Validators.required]] });
  phoneCodeForm = this.fb.group({ verificationCode: ['', [Validators.required]] });

  protected initialize() {
    this.confirmUserService.initialize();

    effect(() => {
      if (this.phone()?.number) {
        this.form.controls.phoneNumber.setValue(this.phone().number);
      }
    });
  }

  constructor() {
    this.initialize();
  }

  sendEmailConfirmation() {
    this.confirmUserService.sendEmailConfirmation();
  }

  sendPhoneConfirmation() {
    this.confirmUserService.sendPhoneConfirmation(this.form.controls.phoneNumber.value);
    this.phoneModalVisible = true;
  }

  verifyPhoneCode() {
    if (this.phoneCodeForm.invalid) {
      return;
    }

    this.confirmUserService.verifyPhoneCode(this.phoneCodeForm.controls.verificationCode.value);
  }
}
