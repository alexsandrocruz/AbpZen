import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SecurityCodeService } from '../services/security-code.service';

/**
 * @deprecated Use `securityCodeGuard` *function* instead.
 */
@Injectable()
export class SecurityCodeGuard {
  constructor(private service: SecurityCodeService, private router: Router) {}

  canActivate() {
    const urlTree = this.router.createUrlTree(['/account/login']);
    return !!this.service.data ? true : urlTree;
  }
}

export const securityCodeGuard = () => {
  const service = inject(SecurityCodeService);
  const router = inject(Router);

  const urlTree = router.createUrlTree(['/account/login']);
  return !!service.data ? true : urlTree;
};
