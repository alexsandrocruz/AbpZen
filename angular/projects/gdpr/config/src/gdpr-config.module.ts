import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CoreModule } from '@abp/ng.core';
import { provideGdprConfig, withCookieConsentOptions } from './providers';
import { AbpCookieConsentOptions } from './models/config-module-options';
import { LocalizeLinkDirective } from './directives/localize-link.directive';
import { GdprCookieConsentComponent } from './components/cookie-consent/gdpr-cookie-consent.component';
import { LinkComponent } from './components/link/link.component';

const declarations = [GdprCookieConsentComponent, LocalizeLinkDirective, LinkComponent];

@NgModule({
  declarations: [...declarations],
  imports: [CoreModule],
  exports: [...declarations, RouterModule],
})
export class GdprConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideAbpCore` *function* for config settings.
   */
  static forRoot(options?: AbpCookieConsentOptions): ModuleWithProviders<GdprConfigModule> {
    return {
      ngModule: GdprConfigModule,
      providers: [provideGdprConfig(withCookieConsentOptions(options))],
    };
  }
}
