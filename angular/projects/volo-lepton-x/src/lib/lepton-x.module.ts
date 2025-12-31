import { ModuleWithProviders, NgModule, Provider } from '@angular/core';

import { LpxStyles } from '@volo/ngx-lepton-x.core';
import { PROFILE_PICTURE_PROVIDERS } from '@volo/abp.commercial.ng.ui/config';
import { LeptonXAbpCoreModule } from '@volo/abp.ng.lepton-x.core';
import { LpxModule, LpxOptions } from '@volosoft/ngx-lepton-x';

import { AbpValidationErrorModule } from './components/abp-validation-error/abp-validation-error.module';
import { LPX_STYLE_PROVIDER } from './providers/style.provider';
import { INIT_SERVICE_PROVIDER } from './providers/init-service.provider';
import {
  LPX_TRANSLATE_KEYS_PROVIDER,
  LPX_TRANSLATE_PROVIDER,
} from './providers/translate.provider';
import { LEPTON_X_USER_MENU_PROVIDERS } from './providers/user-menu-service.provider';
import { ACCOUNT_LAYOUT_PROVIDER } from './providers';
import { httpErrorProvider } from './providers/http-error.provider';

export type ThemeLeptonXModuleOptions = LpxOptions;

@NgModule()
export class ThemeLeptonXModule {
  static forRoot(
    options?: ThemeLeptonXModuleOptions
  ): ModuleWithProviders<ThemeLeptonXModule> {
    return {
      ngModule: ThemeLeptonXModule,
      providers: [
        LPX_STYLE_PROVIDER,
        PROFILE_PICTURE_PROVIDERS,
        LEPTON_X_USER_MENU_PROVIDERS,
        INIT_SERVICE_PROVIDER,
        AbpValidationErrorModule.forRoot().providers as Provider,
        // TODO: muhammed: Create an injection token for theme settings
        LpxModule.forRoot({
          ...createLpxModuleOptions(options),
        }).providers as Provider,
        LPX_TRANSLATE_KEYS_PROVIDER,
        LPX_TRANSLATE_PROVIDER,
        LeptonXAbpCoreModule.forRoot().providers as Provider,
        ACCOUNT_LAYOUT_PROVIDER,
        httpErrorProvider(options),
      ],
    };
  }
}
function createLpxModuleOptions(
  options?: ThemeLeptonXModuleOptions
): LpxOptions {
  return {
    ...options,
    styleFactory: (styles: LpxStyles) => {
      styles.push({
        bundleName: 'abp-bundle',
      });
      if (options?.styleFactory) {
        return options.styleFactory(styles);
      }
      return styles;
    },
  };
}
