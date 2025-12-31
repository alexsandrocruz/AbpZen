import { AuthService, ConfigStateService } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { AccountService } from '../../services/account.service';
import { RecaptchaService } from '../../services/recaptcha.service';
import { PasswordComplexityIndicatorService } from '../../services/password-complexity-indicator.service';
import { RECAPTCHA_STRATEGY } from '../../strategies/recaptcha.strategy';
import { getRedirectUrl } from '../../utils/auth-utils';
import { ProgressBarStats } from '../../models/password-complexity';
const { maxLength, required, email } = Validators;
import { RegisterFormModel } from '../../models/registerFormModel';

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
  providers: [RecaptchaService],
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChild('recaptcha', { static: false })
  recaptchaRef: ElementRef<HTMLDivElement>;

  form: FormGroup<RegisterFormModel>;

  inProgress: boolean;

  isSelfRegistrationEnabled = true;

  showPassword = false;
  capsLock = false;
  progressBar: ProgressBarStats;

  protected fb: FormBuilder;
  protected accountService: AccountService;
  protected toasterService: ToasterService;
  protected configState: ConfigStateService;
  protected authService: AuthService;
  protected recaptchaService: RecaptchaService;
  protected passwordComplexityService: PasswordComplexityIndicatorService;

  constructor(protected injector: Injector) {
    this.fb = injector.get(FormBuilder);
    this.accountService = injector.get(AccountService);
    this.toasterService = injector.get(ToasterService);
    this.configState = injector.get(ConfigStateService);
    this.authService = injector.get(AuthService);
    this.recaptchaService = injector.get(RecaptchaService);
    this.passwordComplexityService = injector.get(PasswordComplexityIndicatorService);
  }
  

  ngOnInit() {
    this.isSelfRegistrationEnabled =
      (
        (this.configState.getSetting('Abp.Account.IsSelfRegistrationEnabled') as string) || ''
      ).toLowerCase() !== 'false';
    if (!this.isSelfRegistrationEnabled) {
      this.toasterService.warn(
        {
          key: 'AbpAccount::SelfRegistrationDisabledMessage',
          defaultValue: 'Self registration is disabled.',
        },
        null,
        { life: 10000 },
      );
      return;
    }

    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, ...getPasswordValidators(this.injector)]],
      email: ['', [required, email]],
    });
  }

  ngAfterViewInit() {
    this.recaptchaService.setStrategy(
      RECAPTCHA_STRATEGY.Register(this.configState, this.recaptchaRef.nativeElement),
    );
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    const newUser = {
      userName: this.form.get('username').value,
      password: this.form.get('password').value,
      emailAddress: this.form.get('email').value,
      appName: 'Angular',
    };

    (this.recaptchaService.isEnabled ? this.recaptchaService.getVerificationToken() : of(undefined))
      .pipe(
        switchMap(captchaResponse =>
          this.accountService.register({ ...newUser, captchaResponse }).pipe(
            switchMap(() =>
              this.authService.login({
                username: newUser.userName,
                password: newUser.password,
                redirectUrl: getRedirectUrl(this.injector) || '/',
              }),
            ),
            catchError(err => {
              this.recaptchaService.reset();
              this.toasterService.error(
                err.error?.error_description ||
                  err.error?.error?.message ||
                  'AbpAccount::DefaultErrorMessage',
                null,
                { life: 7000 },
              );
              return throwError(err);
            }),
          ),
        ),

        finalize(() => (this.inProgress = false)),
      )

      .subscribe();
  }

  get password(): string{
    return this.form.value.password;
  }

  validatePassword(){
    this.progressBar = this.passwordComplexityService.validatePassword(this.password);
  }

}
