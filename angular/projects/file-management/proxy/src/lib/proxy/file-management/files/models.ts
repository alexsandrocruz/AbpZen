import type { ExtensibleAuditedEntityDto, ExtensibleObject } from '@abp/ng.core';
import type { IRemoteStreamContent } from '../../abp/content/models';
import type { FileIconType } from './file-icon-type.enum';

export interface CreateFileInputWithStream extends ExtensibleObject {
  name: string;
  file: IRemoteStreamContent;
  overrideExisting: boolean;
}

export interface DownloadTokenResultDto {
  token?: string;
}

export interface FileDescriptorDto extends ExtensibleAuditedEntityDto<string> {
  directoryId?: string;
  name?: string;
  mimeType?: string;
  size: number;
  concurrencyStamp?: string;
}

export interface FileIconInfo {
  icon?: string;
  type: FileIconType;
}

export interface FileUploadPreInfoDto {
  fileName?: string;
  doesExist: boolean;
  hasValidName: boolean;
}

export interface FileUploadPreInfoRequest {
  directoryId?: string;
  fileName?: string;
  size: number;
}

export interface MoveFileInput {
  id?: string;
  newDirectoryId?: string;
  concurrencyStamp?: string;
}

export interface RenameFileInput {
  name: string;
  concurrencyStamp?: string;
}
