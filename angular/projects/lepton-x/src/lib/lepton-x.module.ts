import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { LpxCoreModule } from '@volo/ngx-lepton-x.core';
import { LpxOptions } from './models/lpx-options';
import { LpxThemeModule } from './theme/theme.module';
import { getLpxProStyleProviders } from './providers';

@NgModule({
  imports: [LpxCoreModule, LpxThemeModule],
})
export class LpxModule {
  public static forRoot(options?: LpxOptions): ModuleWithProviders<LpxModule> {
    return {
      ngModule: LpxModule,
      providers: [
        LpxCoreModule.forRoot(options).providers as Provider,
        LpxThemeModule.forRoot(options?.themeOptions, options?.defaultTheme)
          .providers as Provider,
        ...getLpxProStyleProviders(options?.styleFactory),
      ],
    };
  }
}
