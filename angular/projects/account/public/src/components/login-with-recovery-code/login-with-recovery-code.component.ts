import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { AutofocusDirective, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { RecoveryCodeService } from '../../services';

const { required } = Validators;

@Component({
  standalone: true,
  selector: 'abp-login-with-recovery-code',
  templateUrl: './login-with-recovery-code.component.html',
  imports: [CoreModule, ThemeSharedModule, AutofocusDirective],
})
export class LoginWithRecoveryCodeComponent {
  protected readonly fb = inject(FormBuilder);
  protected readonly recoveryCodeService = inject(RecoveryCodeService);

  protected loading = false;
  protected readonly form = this.fb.group({
    recoveryCode: [null, required],
  });

  login(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.recoveryCodeService
      .login(this.form.value.recoveryCode)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }
}
