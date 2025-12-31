import { mapEnumToOptions } from '@abp/ng.core';

export enum ImportUsersFromFileType {
  Excel = 1,
  Csv = 2,
}

export const importUsersFromFileTypeOptions = mapEnumToOptions(ImportUsersFromFileType);
