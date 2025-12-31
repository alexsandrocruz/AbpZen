import { ModuleWithProviders, NgModule } from '@angular/core';
import { FileManagementConfigModuleOptions } from './models';
import {
  provideFileManagementChildConfig,
  provideFileManagementConfig,
  withUppyOptions,
} from './providers';

/**
 * @deprecated FileManagementConfigModule is deprecated use `provideFileManagementConfig` or `provideFileManagementChildConfig` *function* instead.
 */
@NgModule()
export class FileManagementConfigModule {
  static forRoot(
    options = {} as FileManagementConfigModuleOptions,
  ): ModuleWithProviders<FileManagementConfigModule> {
    return {
      ngModule: FileManagementConfigModule,
      providers: [provideFileManagementConfig(withUppyOptions(options))],
    };
  }

  static forChild(
    options = {} as FileManagementConfigModuleOptions,
  ): ModuleWithProviders<FileManagementConfigModule> {
    return {
      ngModule: FileManagementConfigModule,
      providers: [provideFileManagementChildConfig(withUppyOptions(options))],
    };
  }
}
