import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { OpenIddictProConfigOptions } from './models/config-options';
import { OpeniddictproRoutingModule } from './openiddictpro-routing.module';
import { AdvancedEntityFiltersModule, CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { PageModule } from '@abp/ng.components/page';
import { ApplicationsComponent } from './components/applications/applications.component';

import {
  OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS,
  OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS,
  OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens';
import { ApplicationFormModalComponent } from './components/application-form-modal/application-form-modal.component';
import { ApplicationsService, ScopesService } from './services';
import { ScopesComponent } from './components/scopes/scopes.component';
import { ScopeFormModalComponent } from './components/scope-form-modal/scope-form-modal.component';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { ExtensibleFormComponent, ExtensibleTableComponent } from '@abp/ng.components/extensible';
import { TokenLifetimeModalComponent } from './components';

const components = [
  ApplicationsComponent,
  ApplicationFormModalComponent,
  ScopesComponent,
  ScopeFormModalComponent,
];
@NgModule({
  imports: [
    OpeniddictproRoutingModule,
    CoreModule,
    CommercialUiModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    AdvancedEntityFiltersModule,
    PermissionManagementModule,
    ExtensibleFormComponent,
    TokenLifetimeModalComponent,
    ExtensibleTableComponent,
  ],
  declarations: [...components],
  exports: [...components],
})
export class OpeniddictproModule {
  static forChild(
    options: OpenIddictProConfigOptions = {},
  ): ModuleWithProviders<OpeniddictproModule> {
    return {
      ngModule: OpeniddictproModule,
      providers: [
        {
          provide: OPENIDDICT_PRO_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: OPENIDDICT_PRO_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: OPENIDDICT_PRO_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: OPENIDDICT_PRO_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: OPENIDDICT_PRO_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        ApplicationsService,
        ScopesService,
      ],
    };
  }

  static forLazy(options: OpenIddictProConfigOptions = {}): NgModuleFactory<OpeniddictproModule> {
    return new LazyModuleFactory(OpeniddictproModule.forChild(options));
  }
}
