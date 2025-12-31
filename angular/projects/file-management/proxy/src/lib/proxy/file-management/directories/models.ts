import type { ExtensibleAuditedEntityDto, ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { FileIconInfo } from '../files/models';

export interface CreateDirectoryInput extends ExtensibleObject {
  parentId?: string;
  name: string;
}

export interface DirectoryContentDto extends ExtensibleAuditedEntityDto<string> {
  name?: string;
  isDirectory: boolean;
  size: number;
  iconInfo: FileIconInfo;
  concurrencyStamp?: string;
}

export interface DirectoryContentRequestInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  id?: string;
}

export interface DirectoryDescriptorDto extends ExtensibleAuditedEntityDto<string> {
  name?: string;
  parentId?: string;
  concurrencyStamp?: string;
}

export interface DirectoryDescriptorInfoDto {
  id?: string;
  name?: string;
  parentId?: string;
  hasChildren: boolean;
}

export interface MoveDirectoryInput {
  id?: string;
  newParentId?: string;
  concurrencyStamp?: string;
}

export interface RenameDirectoryInput {
  name: string;
  concurrencyStamp?: string;
}
