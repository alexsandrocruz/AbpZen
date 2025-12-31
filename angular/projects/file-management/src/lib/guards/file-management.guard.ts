import { Inject, Injectable, inject } from '@angular/core';

import { FILE_MANAGEMENT_FEATURES } from '@volo/abp.ng.file-management/common';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ModuleVisibility } from '@volo/abp.commercial.ng.ui/config';

/**
 * @deprecated Use `fileManagementGuard` *function* instead.
 */
@Injectable()
export class FileManagementGuard {
  constructor(
    @Inject(FILE_MANAGEMENT_FEATURES)
    private fileManagementFeatures: Observable<ModuleVisibility>,
  ) {}

  canActivate() {
    return this.fileManagementFeatures.pipe(map(features => features.enable));
  }
}

export const fileManagementGuard = () => {
  const fileManagementFeatures = inject(FILE_MANAGEMENT_FEATURES);

  return fileManagementFeatures.pipe(map(features => features.enable));
};