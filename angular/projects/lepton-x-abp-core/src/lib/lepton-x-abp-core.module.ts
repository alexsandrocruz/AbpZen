import { MOBILE_NAVBAR_ITEMS_FILTER_PROVIDER } from './providers/mobile-navbar-items-filter.provider';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DOCUMENT_DIR_PROVIDER } from './providers/document-dir.provider';
import { IfReplaceableTemplateExistsDirective } from './directives/if-replaceable-template-exists.directive';
import { PageAlertContainerModule } from './components/page-alert-container/page-alert-container.module';
import { PAGE_RENDER_PROVIDER } from './providers/page-render.provider';

@NgModule({
  imports: [CommonModule, PageAlertContainerModule],
  declarations: [IfReplaceableTemplateExistsDirective],
  exports: [IfReplaceableTemplateExistsDirective, PageAlertContainerModule],
})
export class LeptonXAbpCoreModule {
  static forRoot(): ModuleWithProviders<LeptonXAbpCoreModule> {
    return {
      ngModule: LeptonXAbpCoreModule,
      providers: [DOCUMENT_DIR_PROVIDER, MOBILE_NAVBAR_ITEMS_FILTER_PROVIDER,PAGE_RENDER_PROVIDER],
    };
  }
}
