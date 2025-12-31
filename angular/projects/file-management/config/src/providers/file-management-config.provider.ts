import { Provider, makeEnvironmentProviders } from '@angular/core';
import {
  FILE_MANAGEMENT_FEATURES_PROVIDERS,
  UPPY_OPTIONS,
} from '@volo/abp.ng.file-management/common';
import { FILE_MANAGEMENT_ROUTE_PROVIDERS } from './route.provider';
import { FileManagementConfigModuleOptions } from '../models';

export enum FileManagementFeatureKind {
  UppyOptions,
}

export interface FileManagementFeature<KindT extends FileManagementFeatureKind> {
  ɵkind: KindT;
  ɵproviders: Provider[];
}

function makeFileManagementFeature<KindT extends FileManagementFeatureKind>(
  kind: KindT,
  providers: Provider[],
): FileManagementFeature<KindT> {
  return {
    ɵkind: kind,
    ɵproviders: providers,
  };
}

export function withUppyOptions(
  options = {} as FileManagementConfigModuleOptions,
): FileManagementFeature<FileManagementFeatureKind.UppyOptions> {
  return makeFileManagementFeature(FileManagementFeatureKind.UppyOptions, [
    {
      provide: UPPY_OPTIONS,
      useValue: options.uppyOptions,
    },
  ]);
}

export function provideFileManagementConfig(
  ...features: FileManagementFeature<FileManagementFeatureKind>[]
) {
  const providers: Provider[] = [
    FILE_MANAGEMENT_ROUTE_PROVIDERS,
    FILE_MANAGEMENT_FEATURES_PROVIDERS,
    {
      provide: UPPY_OPTIONS,
      useValue: undefined,
    },
  ];

  for (const feature of features) {
    providers.push(...feature.ɵproviders);
  }

  return makeEnvironmentProviders(providers);
}

export function provideFileManagementChildConfig(
  ...features: FileManagementFeature<FileManagementFeatureKind>[]
) {
  return makeEnvironmentProviders([...features.map(f => f.ɵproviders)]);
}
