import { LoginParams, PipeToLoginFn, PIPE_TO_LOGIN_FN_KEY, AuthService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Injectable, Injector } from '@angular/core';
import { throwError, from } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface SecurityCodeData extends LoginParams {
  userId: string;
  twoFactorToken: string;
}

@Injectable()
export class SecurityCodeService {
  data: SecurityCodeData;
  private pipeToLogin?: PipeToLoginFn;
  private authService: AuthService;

  constructor(private injector: Injector, private toaster: ToasterService) {
    this.pipeToLogin = this.injector.get(PIPE_TO_LOGIN_FN_KEY);
    this.authService = this.injector.get(AuthService);
  }

  login(twoFactor: { provider: string; code: string }) {
    const { username, password } = this.data;

    const grantType = 'password';
    const params = {
      username,
      password,
      TwoFactorProvider: twoFactor.provider,
      TwoFactorCode: twoFactor.code,
    };

    const result = this.authService.loginUsingGrant(grantType, params);

    return from(result).pipe(
      this.pipeToLogin &&
        this.pipeToLogin(
          { redirectUrl: this.data.redirectUrl, rememberMe: this.data.rememberMe },
          this.injector,
        ),
      catchError(err => {
        this.toaster.error(
          err.error?.error_description ||
            err.error?.error.message ||
            'AbpAccount::DefaultErrorMessage',
          null,
          { life: 7000 },
        );
        return throwError(() => err);
      }),
    );
  }
}
