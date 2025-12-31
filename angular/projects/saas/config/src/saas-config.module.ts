import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideSaasConfig } from './providers';

@NgModule()
export class SaasConfigModule {
  static forRoot(): ModuleWithProviders<SaasConfigModule> {
    return {
      ngModule: SaasConfigModule,
      providers: [provideSaasConfig()],
    };
  }
}
