import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import {
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbNavModule,
  NgbPopoverModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';

import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { PageModule } from '@abp/ng.components/page';
import { ChartModule } from '@abp/ng.components/chart.js';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { AdvancedEntityFiltersModule, CommercialUiModule } from '@volo/abp.commercial.ng.ui';

import { SaasConfigOptions } from './models';
import {
  EditionsComponent,
  EditionsUsageWidgetComponent,
  LatestTenantsWidgetComponent,
  SetTenantPasswordModalComponent,
  ImpersonateTenantModalComponent,
  TenantsComponent,
} from './components';
import {
  SAAS_CREATE_FORM_PROP_CONTRIBUTORS,
  SAAS_EDIT_FORM_PROP_CONTRIBUTORS,
  SAAS_ENTITY_ACTION_CONTRIBUTORS,
  SAAS_ENTITY_PROP_CONTRIBUTORS,
  SAAS_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import { TENANT_ACTIVATION_STATE_VALIDATOR_PROVIDER } from './validators';
import { SaasRoutingModule } from './saas-routing.module';

const declarationsAndExports = [EditionsUsageWidgetComponent, LatestTenantsWidgetComponent];

export const STANDALONE_COMPONENTS = [
  TenantsComponent,
  ImpersonateTenantModalComponent,
  SetTenantPasswordModalComponent,
];

@NgModule({
  declarations: [...declarationsAndExports],
  exports: [...declarationsAndExports, ...STANDALONE_COMPONENTS],
  imports: [
    SaasRoutingModule,
    NgxValidateCoreModule,
    CoreModule,
    CommercialUiModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgbNavModule,
    NgbPopoverModule,
    NgbDatepickerModule,
    NgbTooltipModule,
    FeatureManagementModule,
    PageModule,
    ChartModule,
    AdvancedEntityFiltersModule,
    EditionsComponent,
    ExtensibleModule,
    ...STANDALONE_COMPONENTS,
  ],
})
export class SaasModule {
  static forChild(options: SaasConfigOptions = {}): ModuleWithProviders<SaasModule> {
    return {
      ngModule: SaasModule,
      providers: [
        TENANT_ACTIVATION_STATE_VALIDATOR_PROVIDER,
        {
          provide: SAAS_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: SAAS_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: SAAS_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: SAAS_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: SAAS_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
      ],
    };
  }

  static forLazy(options: SaasConfigOptions = {}): NgModuleFactory<SaasModule> {
    return new LazyModuleFactory(SaasModule.forChild(options));
  }
}
