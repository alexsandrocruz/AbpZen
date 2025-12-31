import { ModuleWithProviders, NgModule } from '@angular/core';
import {
  LpxAuthLayoutModule,
  LpxAuthLayoutOptions,
} from '@volosoft/ngx-lepton-x/layouts';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { LpxContextMenuModule } from '@volosoft/ngx-lepton-x';
import { AccountLayoutComponent } from './account-layout.component';
import { PageAlertContainerModule } from '@volo/abp.ng.lepton-x.core';
import { AuthWrapperComponent } from './components';

/**
 * @deprecated The interface should not be used
 */
export interface AccountLayoutOptions {
  layout?: LpxAuthLayoutOptions;
}

@NgModule({
  imports: [
    LpxAuthLayoutModule,
    LpxContextMenuModule,
    PageAlertContainerModule,
    CoreModule,
    ThemeSharedModule,
    AuthWrapperComponent,
  ],
  declarations: [AccountLayoutComponent],
})
export class AccountLayoutModule {
  /**
   * @deprecated The method should not be used
   */
  static forRoot(
    options?: AccountLayoutOptions
  ): ModuleWithProviders<AccountLayoutModule> {
    return {
      ngModule: AccountLayoutModule,
      providers: [],
    };
  }
}
