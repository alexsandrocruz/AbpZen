import { ComponentRef, inject, Injectable, Injector } from '@angular/core';
import {
  IdentityUserDelegationService,
  UserDelegationDto,
} from '@volo/abp.ng.account/public/proxy';
import { from } from 'rxjs';
import { AuthService, EnvironmentService, PIPE_TO_LOGIN_FN_KEY } from '@abp/ng.core';
import { AuthorityDelegationComponent } from '../components';
import { Status } from '../models/delegate-authority';

@Injectable({
  providedIn: 'root',
})
export class AbpAuthorityDelegationService {
  private service = inject(IdentityUserDelegationService);
  private authService = inject(AuthService);
  private pipeToLogin = inject(PIPE_TO_LOGIN_FN_KEY);
  private injector = inject(Injector);
  protected readonly environment = inject(EnvironmentService);
  private grantType = 'Impersonation';

  modalRef: ComponentRef<AuthorityDelegationComponent>;

  isVisible() {
    return this.authService.isAuthenticated;
  }

  getUsers() {
    return this.service.getActiveDelegations();
  }

  delegatedImpersonate(userDelegationId: string) {
    const { oAuthConfig } = this.environment.getEnvironment();
    if (oAuthConfig && oAuthConfig.responseType === 'code') {
      this.authService.oidc = false;
    }
    //Todo: Refactor and use existing service in "commercial-ui/config/src/services/impersonation.service.ts" for impersonation
    const promise = this.authService.loginUsingGrant(this.grantType, {
      UserDelegationId: userDelegationId,
    });
    return from(promise).pipe(this.pipeToLogin && this.pipeToLogin({}, this.injector));
  }

  getStatus(row: UserDelegationDto): Status {
    const { startTime, endTime } = row;
    const curr = new Date().getTime();
    const beg = new Date(startTime).getTime();
    const end = new Date(endTime).getTime();

    if (beg > curr) {
      return 'Future';
    } else if (curr > end) {
      return 'Expired';
    } else if (beg < curr && curr < end) {
      return 'Active';
    }
  }

  statusClass(row: UserDelegationDto): string {
    const status = this.getStatus(row);
    switch (status) {
      case 'Active':
        return 'success';
      case 'Expired':
        return 'danger';
      case 'Future':
        return 'warning';
    }
  }
}
