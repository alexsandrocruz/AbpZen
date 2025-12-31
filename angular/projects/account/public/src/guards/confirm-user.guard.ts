import { inject } from '@angular/core';
import { ConfirmUserService } from '../services';

export const confirmUserGuard = () => {
  return inject(ConfirmUserService).validateUserId();
};
