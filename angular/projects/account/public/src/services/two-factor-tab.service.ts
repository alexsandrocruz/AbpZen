import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService, ProfileService } from '@volo/abp.ng.account/public/proxy';

@Injectable()
export class TwoFactorTabService {
  protected readonly accountService = inject(AccountService);
  protected readonly profileService = inject(ProfileService);

  get isTwoFactorEnabled$(): Observable<boolean> {
    return this.profileService.getTwoFactorEnabled();
  }

  setTwoFactorStatus(status: boolean): Observable<void> {
    return this.profileService.setTwoFactorEnabled(status);
  }
}
