import { Inject, Injectable, inject } from '@angular/core';

import { TEXT_TEMPLATE_MANAGEMENT_FEATURES } from '@volo/abp.ng.text-template-management/common';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ModuleVisibility } from '@volo/abp.commercial.ng.ui/config';

/**
 * @deprecated Use `textTemplateManagementGuard` *function* instead.
 */
@Injectable()
export class TextTemplateManagementGuard {
  constructor(
    @Inject(TEXT_TEMPLATE_MANAGEMENT_FEATURES)
    private textTemplateManagementFeatures: Observable<ModuleVisibility>,
  ) {}

  canActivate() {
    return this.textTemplateManagementFeatures.pipe(map(features => features.enable));
  }
}

export const textTemplateManagementGuard = () => {
  const textTemplateManagementFeatures = inject(TEXT_TEMPLATE_MANAGEMENT_FEATURES);

  return textTemplateManagementFeatures.pipe(map(features => features.enable));
};
