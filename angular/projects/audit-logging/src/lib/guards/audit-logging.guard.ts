import { Inject, Injectable, inject } from '@angular/core';

import { AUDIT_LOGGING_FEATURES } from '@volo/abp.ng.audit-logging/common';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ModuleVisibility } from '@volo/abp.commercial.ng.ui/config';

/**
 * @deprecated Use `auditLoggingGuard` *function* instead.
 */
@Injectable()
export class AuditLoggingGuard {
  constructor(
    @Inject(AUDIT_LOGGING_FEATURES) private auditLoggingFeatures: Observable<ModuleVisibility>,
  ) {}

  canActivate() {
    return this.auditLoggingFeatures.pipe(map(features => features.enable));
  }
}

export const auditLoggingGuard = () => {
  const auditLoggingFeatures = inject(AUDIT_LOGGING_FEATURES);

  return auditLoggingFeatures.pipe(map(features => features.enable));
};
