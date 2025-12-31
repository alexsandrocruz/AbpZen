import {
  APP_INITIALIZER,
  Injector,
  ModuleWithProviders,
  NgModule,
  Provider,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule, ReplaceableComponentsService } from '@abp/ng.core';
import { LpxSideMenuLayoutModule } from '@volosoft/ngx-lepton-x/layouts';
import {
  AbpAuthService,
  eThemeLeptonXComponents,
} from '@volosoft/abp.ng.theme.lepton-x';
import { SideMenuApplicationLayoutComponent } from './side-menu-application-layout.component';
import {
  LPX_AUTH_SERVICE_TOKEN,
  LpxBreadcrumbModule,
  LpxFooterModule,
  PanelsModule,
} from '@volo/ngx-lepton-x.core';
import { LPX_TRANSLATE_KEYS_PROVIDER } from './providers/translate.provider';
import { LeptonXAbpCoreModule } from '@volo/abp.ng.lepton-x.core';
import { EmptyLayoutComponent } from '../../empty-layout';

@NgModule({
  declarations: [SideMenuApplicationLayoutComponent],
  imports: [
    LeptonXAbpCoreModule,
    CommonModule,
    CoreModule,
    LpxSideMenuLayoutModule,
    LpxBreadcrumbModule,
    PanelsModule,
    LpxFooterModule,
  ],
  exports: [SideMenuApplicationLayoutComponent],
})
export class SideMenuLayoutModule {
  static forRoot(): ModuleWithProviders<SideMenuLayoutModule> {
    return {
      ngModule: SideMenuLayoutModule,
      providers: [
        LpxSideMenuLayoutModule.forRoot().providers as Provider,
        {
          provide: APP_INITIALIZER,
          useFactory: initLayouts,
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

export function initLayouts(injector: Injector) {
  function init() {
    const replaceableComponents = injector.get(ReplaceableComponentsService);
    replaceableComponents.add({
      key: eThemeLeptonXComponents.ApplicationLayout,
      component: SideMenuApplicationLayoutComponent,
    });
    replaceableComponents.add({
      key: eThemeLeptonXComponents.EmptyLayout,
      component: EmptyLayoutComponent,
    });
  }
  return init;
}
