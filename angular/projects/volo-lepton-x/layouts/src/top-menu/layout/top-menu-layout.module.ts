import {
  APP_INITIALIZER,
  Injector,
  ModuleWithProviders,
  NgModule,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopMenuApplicationLayoutComponent } from './top-menu-application-layout.component';
import {
  LPX_AUTH_SERVICE_TOKEN,
  LpxBreadcrumbModule,
  LpxFooterModule,
  LpxNavbarModule,
} from '@volo/ngx-lepton-x.core';
import {
  AbpAuthService,
  eThemeLeptonXComponents,
} from '@volosoft/abp.ng.theme.lepton-x';
import { LPX_TRANSLATE_KEYS_PROVIDER } from './providers';
import { ReplaceableComponentsService, CoreModule } from '@abp/ng.core';
import { LpxTopMenuLayoutModule } from '@volosoft/ngx-lepton-x/layouts';
import {
  LeptonXAbpCoreModule,
  PageAlertContainerModule,
} from '@volo/abp.ng.lepton-x.core';
import { RouterOutlet } from '@angular/router';
import { EmptyLayoutComponent } from '../../empty-layout';

@NgModule({
  declarations: [TopMenuApplicationLayoutComponent],
  imports: [
    CommonModule,
    LpxTopMenuLayoutModule,
    LeptonXAbpCoreModule,
    LpxBreadcrumbModule,
    LpxFooterModule,
    LpxNavbarModule,
    CoreModule,
    PageAlertContainerModule,
    RouterOutlet,
  ],
})
export class TopMenuLayoutModule {
  static forRoot(): ModuleWithProviders<TopMenuLayoutModule> {
    return {
      ngModule: TopMenuLayoutModule,
      providers: [
        LpxTopMenuLayoutModule.forRoot().providers,
        {
          provide: APP_INITIALIZER,
          useFactory: initLayoutsFactory,
          deps: [Injector],
          multi: true,
        },
        {
          provide: LPX_AUTH_SERVICE_TOKEN,
          useClass: AbpAuthService,
        },
        LPX_TRANSLATE_KEYS_PROVIDER,
      ],
    };
  }
}

export function initLayoutsFactory(injector: Injector) {
  function init() {
    const replaceableComponents = injector.get(ReplaceableComponentsService);
    replaceableComponents.add({
      key: eThemeLeptonXComponents.ApplicationLayout,
      component: TopMenuApplicationLayoutComponent,
    });
    replaceableComponents.add({
      key: eThemeLeptonXComponents.EmptyLayout,
      component: EmptyLayoutComponent,
    });
  }

  return init;
}
