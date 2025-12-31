import { inject, Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { IdentityUserService } from '@volo/abp.ng.identity/proxy';
import { finalize } from 'rxjs/operators';

@Injectable()
export class UserLockService {
  protected readonly fb = inject(FormBuilder);
  protected readonly proxyService = inject(IdentityUserService);
  busy = false;

  buildLockForm() {
    return this.fb.group({
      lockoutEnd: [new Date().toISOString(), [Validators.required]],
    });
  }

  lock(userId: string | null, lockoutEnd: string) {
    this.busy = true;
    return this.proxyService.lock(userId, lockoutEnd).pipe(finalize(() => (this.busy = false)));
  }
}
