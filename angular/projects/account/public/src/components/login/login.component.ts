import {
  AfterViewInit,
  Component,
  ElementRef,
  Injector,
  OnInit,
  ViewChild,
  inject,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, ConfigStateService, LoginParams } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { IdentityLinkUserService, LinkUserInput } from '@volo/abp.ng.account/public/proxy';
import { from, of, pipe, throwError } from 'rxjs';
import { catchError, finalize, switchMap, tap } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';
import { RecaptchaService } from '../../services/recaptcha.service';
import { SecurityCodeService } from '../../services/security-code.service';
import { RecoveryCodeService } from '../../services/recovery-code.service';
import { RECAPTCHA_STRATEGY } from '../../strategies/recaptcha.strategy';
import { getRedirectUrl } from '../../utils/auth-utils';
import {
  PERIODICALLY_CHANGE_PASSWORD,
  REQUIRES_TWO_FACTOR,
  SHOULD_CHANGE_PASSWORD_ON_NEXT_LOGIN,
  REQUIRES_CONFIRM_USER,
} from '../../enums';
import { ConfirmUserService } from '../../services';

const { maxLength, required } = Validators;

@Component({
  selector: 'abp-login',
  templateUrl: './login.component.html',
  providers: [RecaptchaService],
})
export class LoginComponent implements OnInit, AfterViewInit {
  protected readonly injector = inject(Injector);
  protected readonly fb = inject(FormBuilder);
  protected readonly toasterService = inject(ToasterService);
  protected readonly authService = inject(AuthService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly identityLinkUserService = inject(IdentityLinkUserService);
  protected readonly recaptchaService = inject(RecaptchaService);
  protected readonly securityCodeService = inject(SecurityCodeService);
  protected readonly recoveryCodeService = inject(RecoveryCodeService);
  protected readonly confirmUserService = inject(ConfirmUserService);

  @ViewChild('recaptcha', { static: false })
  recaptchaRef: ElementRef<HTMLDivElement>;

  form = this.buildForm();

  inProgress: boolean;

  isSelfRegistrationEnabled = true;

  authWrapperKey = eAccountComponents.AuthWrapper;

  linkUser: LinkUserInput;

  showPassword = false;
  capsLock = false;

  protected init() {
    this.isSelfRegistrationEnabled =
      (
        (this.configState.getSetting('Abp.Account.IsSelfRegistrationEnabled') as string) || ''
      ).toLowerCase() !== 'false';
  }

  protected buildForm() {
    return this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, maxLength(128)]],
      rememberMe: [false],
    });
  }

  protected setLinkUserParams() {
    const {
      linkUserId: userId,
      linkToken: token,
      linkTenantId: tenantId,
    } = this.route.snapshot.queryParams;

    if (userId && token) {
      this.identityLinkUserService.verifyLinkToken({ token, userId, tenantId }).subscribe(res => {
        if (res) {
          this.linkUser = { userId, token, tenantId };
        }
      });
    }
  }

  ngOnInit() {
    this.init();
    this.setLinkUserParams();
  }

  ngAfterViewInit() {
    this.recaptchaService.setStrategy(
      RECAPTCHA_STRATEGY.Login(this.configState, this.recaptchaRef.nativeElement),
    );
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    const { username, password, rememberMe } = this.form.value;
    const redirectUrl = getRedirectUrl(this.injector) || (this.linkUser ? null : '/');
    const loginParams = { username, password, rememberMe, redirectUrl };

    this.checkRecaptcha()
      .pipe(
        switchMap(isValid => {
          if (!isValid) {
            return of(null);
          }

          return this.authService
            .login(loginParams)
            .pipe(this.handleLoginError(loginParams))
            .pipe(this.linkUser ? this.switchToLinkUser() : tap());
        }),
        finalize(() => (this.inProgress = false)),
      )
      .subscribe();
  }

  private checkRecaptcha() {
    return this.recaptchaService.isEnabled ? this.recaptchaService.validate() : of(true);
  }

  private switchToLinkUser() {
    return pipe(
      switchMap(() => this.identityLinkUserService.link(this.linkUser)),
      tap(() => {
        this.router.navigate(['/account/link-logged'], {
          queryParams: this.route.snapshot.queryParams,
        });
      }),
    );
  }

  private handleLoginError(loginParams?: LoginParams) {
    return catchError(err => {
      const errorDescription = err.error?.error_description;

      switch (errorDescription) {
        case REQUIRES_TWO_FACTOR:
          const data = {
            ...loginParams,
            userId: err.error.userId,
          };
          this.securityCodeService.data = { ...data, twoFactorToken: err.error.twoFactorToken };
          this.recoveryCodeService.data.set({ ...data });
          return from(this.router.navigate(['/account/send-security-code']));
        case PERIODICALLY_CHANGE_PASSWORD:
        case SHOULD_CHANGE_PASSWORD_ON_NEXT_LOGIN: {
          const queryParams = {
            token: err.error.changePasswordToken,
            redirectUrl: loginParams.redirectUrl,
            username: loginParams.username,
          };
          return from(
            this.router.navigate(['/account/change-password'], {
              queryParams,
            }),
          );
        }
        case REQUIRES_CONFIRM_USER: {
          const { userId, email, phoneNumber } = err?.error ?? {};
          if (!err || !err.error) {
            throw new Error(`Error detail could not be found.\n ${err}`);
          }

          this.confirmUserService.update(prev => ({
            userId,
            email: { ...prev.email, address: email },
            phone: { ...prev.phone, number: phoneNumber },
          }));
          return from(this.router.navigate(['/account/confirm-user']));
        }
      }

      this.recaptchaService.reset();
      this.toasterService.error(
        err.error?.error_description ||
          err.error?.error?.message ||
          'AbpAccount::DefaultErrorMessage',
        null,
        { life: 7000 },
      );

      return throwError(() => err);
    });
  }
}
