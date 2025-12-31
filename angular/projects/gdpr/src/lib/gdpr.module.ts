import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { GdprRoutingModule } from './gdpr-routing.module';
import { GdprComponent } from './components/gdpr.component';
import { GdprConfigOptions } from './models';
import { DEFAULT_GDPR_ENTITY_PROPS, DEFAULT_GDPR_TOOLBAR_ACTIONS } from './tokens/extensions.token';
import { PageModule } from '@abp/ng.components/page';
import { GdprActionColumnComponent } from './components/gdpr-action-column/gdpr-action-column.component';
import { RequestPersonalDataToolbarActionComponent } from './components/request-personal-data-toolbar-action/request-personal-data-toolbar-action.component';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';

const declarations = [
  GdprComponent,
  GdprActionColumnComponent,
  RequestPersonalDataToolbarActionComponent,
];

@NgModule({
  declarations: [...declarations],
  imports: [
    CoreModule,
    ThemeSharedModule,
    GdprRoutingModule,
    ExtensibleModule,
    PageModule,
    NgbTooltipModule,
  ],
  exports: [...declarations],
})
export class GdprModule {
  static forChild(options: GdprConfigOptions = {}): ModuleWithProviders<GdprModule> {
    return {
      ngModule: GdprModule,
      providers: [
        {
          provide: DEFAULT_GDPR_ENTITY_PROPS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: DEFAULT_GDPR_TOOLBAR_ACTIONS,
          useValue: options.toolbarActionContributors,
        },
      ],
    };
  }

  static forLazy(): NgModuleFactory<GdprModule> {
    return new LazyModuleFactory(GdprModule.forChild());
  }
}
