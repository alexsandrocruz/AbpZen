import { inject, Injectable, Injector } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { comparePasswords, Validation } from '@ngx-validate/core';
import { AuthService } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { ChangePasswordInput, ProfileService } from '@volo/abp.ng.account/public/proxy';
import { ManageProfileStateService } from '../services/manage-profile-state.service';

const { required } = Validators;

@Injectable({
  providedIn: 'root',
})
export class ChangePasswordService {
  protected readonly manageProfileState = inject(ManageProfileStateService);
  protected readonly fb = inject(FormBuilder);
  protected readonly toasterService = inject(ToasterService);
  protected readonly activatedRoute = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly profileService = inject(ProfileService);
  protected readonly authService = inject(AuthService);

  protected readonly toasterSuccessOption = {
    message: 'AbpAccount::PasswordChangedMessage',
    title: '',
    options: { life: 5000 },
  };

  private legacyInjector = inject(Injector);
  public readonly PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];

  private getQueryParams() {
    const { token, username } = this.activatedRoute.snapshot.queryParams;
    return { token, username };
  }

  public readonly MapErrorsFnFactory: () => Validation.MapErrorsFn =
    () => (errors, groupErrors, control) => {
      if (this.PASSWORD_FIELDS.indexOf(String(control.name)) < 0) return errors;
      return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
    };

  get hasPassword() {
    return !!this.manageProfileState.getProfile()?.hasPassword;
  }

  private get returnUrl() {
    return this.activatedRoute.snapshot.queryParams.returnUrl;
  }

  buildForm(hideCurrentPassword: boolean = false) {
    const passwordValidations = getPasswordValidators(this.legacyInjector);

    const form = this.fb.group(
      {
        currentPassword: ['', required],
        newPassword: [
          '',
          {
            validators: [required, ...passwordValidations],
          },
        ],
        repeatNewPassword: [
          '',
          {
            validators: [required, ...passwordValidations],
          },
        ],
      },
      {
        validators: [comparePasswords(this.PASSWORD_FIELDS)],
      },
    );
    if (hideCurrentPassword) {
      form.removeControl('currentPassword');
    }

    return form;
  }

  changePassword(formValue: ChangePasswordInput): Observable<any> {
    return this.profileService.changePassword(formValue);
  }

  changePasswordAndLogin(formValue: ChangePasswordInput) {
    const queryParams = this.getQueryParams();

    const p = {
      password: formValue.currentPassword,
      NewPassword: formValue.newPassword,
      username: queryParams.username,
      ChangePasswordToken: queryParams.token,
    };
    return from(this.authService.loginUsingGrant('password', p));
  }
  redirectToReturnUrl() {
    const url = this.returnUrl || '/';
    return from(this.router.navigateByUrl(url));
  }
  public showSuccessMessage() {
    const { message, title, options } = this.toasterSuccessOption;
    this.toasterService.success(message, title, options);
  }

  public showErrorMessage(err: { error?: { error?: { message: string } } }) {
    this.toasterService.error(err.error?.error?.message || 'AbpAccount::DefaultErrorMessage');
  }
}
