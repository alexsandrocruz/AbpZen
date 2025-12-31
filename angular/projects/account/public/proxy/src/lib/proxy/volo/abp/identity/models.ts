import type {
  EntityDto,
  ExtensibleFullAuditedEntityDto,
  ExtensiblePagedAndSortedResultRequestDto,
} from '@abp/ng.core';

export interface GetIdentitySecurityLogListInput extends ExtensiblePagedAndSortedResultRequestDto {
  startTime?: string;
  endTime?: string;
  applicationName?: string;
  identity?: string;
  action?: string;
  userName?: string;
  clientId?: string;
  correlationId?: string;
}

export interface IdentitySecurityLogDto extends EntityDto<string> {
  tenantId?: string;
  applicationName?: string;
  identity?: string;
  action?: string;
  userId?: string;
  userName?: string;
  tenantName?: string;
  clientId?: string;
  correlationId?: string;
  clientIpAddress?: string;
  browserInfo?: string;
  creationTime?: string;
  extraProperties: Record<string, object>;
}

export interface IdentitySessionDto {
  id?: string;
  sessionId?: string;
  isCurrent: boolean;
  device?: string;
  deviceInfo?: string;
  tenantId?: string;
  tenantName?: string;
  userId?: string;
  userName?: string;
  clientId?: string;
  ipAddresses: string[];
  signedIn?: string;
  lastAccessed?: string;
}

export interface IdentityUserDto extends ExtensibleFullAuditedEntityDto<string> {
  tenantId?: string;
  userName?: string;
  email?: string;
  name?: string;
  surname?: string;
  emailConfirmed: boolean;
  phoneNumber?: string;
  phoneNumberConfirmed: boolean;
  supportTwoFactor: boolean;
  twoFactorEnabled: boolean;
  isActive: boolean;
  lockoutEnabled: boolean;
  isLockedOut: boolean;
  lockoutEnd?: string;
  shouldChangePasswordOnNextLogin: boolean;
  concurrencyStamp?: string;
  roleNames: string[];
  accessFailedCount: number;
  lastPasswordChangeTime?: string;
  isExternal: boolean;
}
