import type {
  ExtensibleEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/ng.core';

export interface CreateScopeInput extends ScopeCreateOrUpdateDtoBase {}

export interface GetScopeListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface ScopeCreateOrUpdateDtoBase extends ExtensibleObject {
  name: string;
  displayName?: string;
  description?: string;
  resources: string[];
}

export interface ScopeDto extends ExtensibleEntityDto<string> {
  name?: string;
  displayName?: string;
  description?: string;
  buildIn: boolean;
  resources: string[];
}

export interface UpdateScopeInput extends ScopeCreateOrUpdateDtoBase {}
