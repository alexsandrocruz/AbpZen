import { Injectable } from '@angular/core';
import { AuthService, UserProfileService } from '@volo/ngx-lepton-x.core';
import { map } from 'rxjs/operators';
import { AuthService as AuthCoreService } from '@abp/ng.core';

@Injectable()
export class AbpAuthService implements AuthService {
  constructor(
    private userProfileService: UserProfileService,
    private authService: AuthCoreService
  ) {}

  isUserExists$ = this.userProfileService.user$.pipe(
    map((user) => user && !!user.userName)
  );

  navigateToLogin(): void {
    this.authService.navigateToLogin();
  }
}
