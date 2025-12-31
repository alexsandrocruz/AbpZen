import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AccountService } from '../../services/account.service';
const { required, email } = Validators;

@Component({
  selector: 'abp-forgot-password',
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  form: UntypedFormGroup;

  inProgress: boolean;

  isEmailSent: boolean = false;

  constructor(private fb: UntypedFormBuilder, private accountService: AccountService) {
    this.form = this.fb.group({
      email: ['', [required, email]],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    this.accountService
      .sendPasswordResetCode({ email: this.form.get('email').value, appName: 'Angular' })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.isEmailSent = true;
      });
  }
}
