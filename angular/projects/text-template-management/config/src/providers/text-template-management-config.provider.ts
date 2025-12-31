import { makeEnvironmentProviders } from '@angular/core';
import { TEXT_TEMPLATE_MANAGEMENT_FEATURES_PROVIDERS } from '@volo/abp.ng.text-template-management/common';
import { TEXT_TEMPLATE_MANAGEMENT_ROUTE_PROVIDERS } from './';

export function provideTextTemplateManagementConfig() {
  return makeEnvironmentProviders([
    TEXT_TEMPLATE_MANAGEMENT_ROUTE_PROVIDERS,
    TEXT_TEMPLATE_MANAGEMENT_FEATURES_PROVIDERS,
  ]);
}
