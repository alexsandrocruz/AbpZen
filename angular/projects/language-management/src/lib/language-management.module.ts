import { PageModule } from '@abp/ng.components/page';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AdvancedEntityFiltersModule, CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { LanguageTextsComponent } from './components/language-texts/language-texts.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { LanguageManagementRoutingModule } from './language-management-routing.module';
import { LanguageManagementConfigOptions } from './models/config-options';
import {
  LANGUAGE_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  LANGUAGE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS
} from './tokens/extensions.token';

@NgModule({
  declarations: [LanguagesComponent, LanguageTextsComponent],
  exports: [LanguagesComponent, LanguageTextsComponent],
  imports: [
    LanguageManagementRoutingModule,
    CoreModule,
    CommercialUiModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    AdvancedEntityFiltersModule,
  ],
})
export class LanguageManagementModule {
  static forChild(
    options: LanguageManagementConfigOptions = {},
  ): ModuleWithProviders<LanguageManagementModule> {
    return {
      ngModule: LanguageManagementModule,
      providers: [
        {
          provide: LANGUAGE_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: LANGUAGE_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: LANGUAGE_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: LANGUAGE_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: LANGUAGE_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
      ],
    };
  }

  static forLazy(
    options: LanguageManagementConfigOptions = {},
  ): NgModuleFactory<LanguageManagementModule> {
    return new LazyModuleFactory(LanguageManagementModule.forChild(options));
  }
}
