import type { NameValue } from '../volo/abp/models';

export interface ExternalProviderDto {
  providers: ExternalProviderItemDto[];
}

export interface ExternalProviderItemDto {
  name?: string;
  enabled: boolean;
  properties: ExternalProviderSettingsProperty[];
}

export interface ExternalProviderItemWithSecretDto {
  success: boolean;
  name?: string;
  enabled: boolean;
  properties: ExternalProviderSettingsProperty[];
  secretProperties: ExternalProviderSettingsProperty[];
}

export interface ExternalProviderSettingsProperty extends NameValue<string> {
}

export interface GetByNameInput {
  tenantId?: string;
  name?: string;
}
