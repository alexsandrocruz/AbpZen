import { Injectable } from '@angular/core';
import { AuthService } from './models';

import { UserProfileService } from '../user-profile';
import { map } from 'rxjs/operators';

@Injectable()
export class DefaultAuthService implements AuthService {
  constructor(private userProfileService: UserProfileService) {}
  isUserExists$ = this.userProfileService.user$.pipe(
    map((user) => !!user && Object.keys(user).length > 0)
  );

  navigateToLogin(): void {
    return;
  }
}
