import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { EntityChangeDetailsComponent } from './components/entity-change-details.component';
import { EntityChangeModalComponent } from './components/entity-change-modal.component';
import { provideAuditLoggingConfig } from './providers';

@NgModule({
  declarations: [EntityChangeDetailsComponent, EntityChangeModalComponent],
  exports: [EntityChangeDetailsComponent, EntityChangeModalComponent],
  imports: [CoreModule, ThemeSharedModule],
})
export class AuditLoggingConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideAuditLoggingConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<AuditLoggingConfigModule> {
    return {
      ngModule: AuditLoggingConfigModule,
      providers: [provideAuditLoggingConfig()],
    };
  }
}
