import type {
  ExtensibleEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/ng.core';
import type { TenantActivationState } from '../../tenant-activation-state.enum';

export interface EditionCreateDto extends EditionCreateOrUpdateDtoBase {}

export interface EditionCreateOrUpdateDtoBase extends ExtensibleObject {
  displayName: string;
  planId?: string;
}

export interface EditionDto extends ExtensibleEntityDto<string> {
  displayName?: string;
  planId?: string;
  planName?: string;
  concurrencyStamp?: string;
  tenantCount: number;
}

export interface EditionLookupDto extends ExtensibleEntityDto<string> {
  displayName?: string;
}

export interface EditionUpdateDto extends EditionCreateOrUpdateDtoBase {
  concurrencyStamp?: string;
}

export interface GetEditionsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface GetTenantsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  getEditionNames: boolean;
  editionId?: string;
  expirationDateMin?: string;
  expirationDateMax?: string;
  activationState?: TenantActivationState;
}

export interface SaasTenantConnectionStringsDto extends ExtensibleEntityDto {
  default?: string;
  databases: SaasTenantDatabaseConnectionStringsDto[];
}

export interface SaasTenantCreateDto extends SaasTenantCreateOrUpdateDtoBase {
  adminEmailAddress: string;
  adminPassword: string;
  connectionStrings: SaasTenantConnectionStringsDto;
}

export interface SaasTenantCreateOrUpdateDtoBase extends ExtensibleObject {
  name: string;
  editionId?: string;
  activationState: TenantActivationState;
  activationEndDate?: string;
  editionEndDateUtc?: string;
}

export interface SaasTenantDatabaseConnectionStringsDto extends ExtensibleEntityDto {
  databaseName?: string;
  connectionString?: string;
}

export interface SaasTenantDatabasesDto extends ExtensibleEntityDto {
  databases: string[];
}

export interface SaasTenantDto extends ExtensibleEntityDto<string> {
  name?: string;
  editionId?: string;
  editionEndDateUtc?: string;
  editionName?: string;
  hasDefaultConnectionString: boolean;
  activationState: TenantActivationState;
  activationEndDate?: string;
  concurrencyStamp?: string;
}

export interface SaasTenantSetPasswordDto {
  username?: string;
  password?: string;
}

export interface SaasTenantUpdateDto extends SaasTenantCreateOrUpdateDtoBase {
  concurrencyStamp?: string;
}
