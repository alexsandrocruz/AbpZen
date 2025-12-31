import type {
  ExtensibleEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/ng.core';

export interface ApplicationCreateOrUpdateDtoBase extends ExtensibleObject {
  applicationType: string;
  clientId: string;
  displayName: string;
  clientType?: string;
  clientSecret?: string;
  consentType?: string;
  extensionGrantTypes: string[];
  postLogoutRedirectUris: string[];
  redirectUris: string[];
  allowPasswordFlow: boolean;
  allowClientCredentialsFlow: boolean;
  allowAuthorizationCodeFlow: boolean;
  allowRefreshTokenFlow: boolean;
  allowHybridFlow: boolean;
  allowImplicitFlow: boolean;
  allowLogoutEndpoint: boolean;
  allowDeviceEndpoint: boolean;
  scopes: string[];
  clientUri?: string;
  logoUri?: string;
}

export interface ApplicationDto extends ExtensibleEntityDto<string> {
  applicationType?: string;
  clientId?: string;
  displayName?: string;
  clientType?: string;
  clientSecret?: string;
  consentType?: string;
  extensionGrantTypes: string[];
  postLogoutRedirectUris: string[];
  redirectUris: string[];
  allowPasswordFlow: boolean;
  allowClientCredentialsFlow: boolean;
  allowAuthorizationCodeFlow: boolean;
  allowRefreshTokenFlow: boolean;
  allowHybridFlow: boolean;
  allowImplicitFlow: boolean;
  allowLogoutEndpoint: boolean;
  allowDeviceEndpoint: boolean;
  scopes: string[];
  clientUri?: string;
  logoUri?: string;
}

export interface ApplicationTokenLifetimeDto {
  accessTokenLifetime?: number;
  authorizationCodeLifetime?: number;
  deviceCodeLifetime?: number;
  identityTokenLifetime?: number;
  refreshTokenLifetime?: number;
  userCodeLifetime?: number;
}

export interface CreateApplicationInput extends ApplicationCreateOrUpdateDtoBase {}

export interface GetApplicationListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UpdateApplicationInput extends ApplicationCreateOrUpdateDtoBase {}
