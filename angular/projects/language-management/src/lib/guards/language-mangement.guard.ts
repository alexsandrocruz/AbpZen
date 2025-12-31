import { Inject, Injectable, inject } from '@angular/core';

import { LANGUAGE_MANAGEMENT_FEATURES } from '@volo/abp.ng.language-management/common';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ModuleVisibility } from '@volo/abp.commercial.ng.ui/config';

/**
 * @deprecated Use `languageManagementGuard` *function* instead.
 */
@Injectable()
export class LanguageManagementGuard {
  constructor(
    @Inject(LANGUAGE_MANAGEMENT_FEATURES)
    private languageManagementFeatures: Observable<ModuleVisibility>,
  ) {}

  canActivate() {
    return this.languageManagementFeatures.pipe(map(features => features.enable));
  }
}

export const languageManagementGuard = () => {
  const languageManagementFeatures = inject(LANGUAGE_MANAGEMENT_FEATURES);

  return languageManagementFeatures.pipe(map(features => features.enable));
};
