import { AuthService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

/**
 * @deprecated Use `authenticationFlowGuard` *function* instead.
 */
@Injectable()
export class AuthenticationFlowGuard {
  constructor(private authService: AuthService) {}

  canActivate() {
    if (this.authService.isInternalAuth) return true;

    this.authService.navigateToLogin();
    return false;
  }
}

export const authenticationFlowGuard = () => {
  const authService = inject(AuthService);

  if (authService.isInternalAuth) return true;

  authService.navigateToLogin();
  return false;
};