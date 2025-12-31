import { Injectable, Injector, computed, inject, signal } from '@angular/core';
import { catchError, from, throwError } from 'rxjs';
import { AuthService, PIPE_TO_LOGIN_FN_KEY, PipeToLoginFn } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { RecoveryCodeData } from '../models';

@Injectable()
export class RecoveryCodeService {
  private readonly pipeToLogin? = inject<PipeToLoginFn>(PIPE_TO_LOGIN_FN_KEY);

  protected readonly injector = inject(Injector);
  protected readonly authService = inject(AuthService);
  protected readonly toaster = inject(ToasterService);

  data = signal<RecoveryCodeData>(null);
  hasData = computed(() => !!this.data());

  login(recoveryCode: string) {
    const { username, password, redirectUrl, rememberMe } = this.data();

    const grantType = 'password';
    const params = {
      username,
      password,
      RecoveryCode: recoveryCode,
    };

    const result = this.authService.loginUsingGrant(grantType, params);

    return from(result).pipe(
      this.pipeToLogin && this.pipeToLogin({ redirectUrl, rememberMe }, this.injector),
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
