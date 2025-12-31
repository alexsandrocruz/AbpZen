import { Injectable, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';
import { AccountService, VerifyAuthenticatorCodeDto } from '@volo/abp.ng.account/public/proxy';

@Injectable()
export class AuthenticatorAppService {
  protected readonly accountService = inject(AccountService);

  hasAuthenticator = toSignal(this.accountService.hasAuthenticator());
  authenticatorInfo = toSignal(this.accountService.getAuthenticatorInfo());

  resetAuthenticator(): Observable<void> {
    return this.accountService.resetAuthenticator();
  }

  verifyAuthenticatorCode(code: string): Observable<VerifyAuthenticatorCodeDto> {
    return this.accountService.verifyAuthenticatorCode({ code });
  }
}
