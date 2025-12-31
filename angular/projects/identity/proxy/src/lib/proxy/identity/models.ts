import type {
  CreationAuditedEntityDto,
  EntityDto,
  ExtensibleEntityDto,
  ExtensibleFullAuditedEntityDto,
  ExtensibleObject,
  ExtensiblePagedAndSortedResultRequestDto,
  ExtensiblePagedResultRequestDto,
} from '@abp/ng.core';
import type { IdentityClaimValueType } from './identity-claim-value-type.enum';
import type { ImportUsersFromFileType } from './import-users-from-file-type.enum';
import type { IdentityProPreventConcurrentLoginBehaviour } from './settings/identity-pro-prevent-concurrent-login-behaviour.enum';
import type { IRemoteStreamContent } from '../content/models';

export interface ClaimTypeDto extends ExtensibleEntityDto<string> {
  name?: string;
  required: boolean;
  isStatic: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
  valueType: IdentityClaimValueType;
  valueTypeAsString?: string;
  concurrencyStamp?: string;
}

export interface CreateClaimTypeDto extends ExtensibleObject {
  name: string;
  required: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
  valueType: IdentityClaimValueType;
}

export interface DownloadTokenResultDto {
  token?: string;
}

export interface ExternalLoginProviderDto {
  name?: string;
  canObtainUserInfoWithoutPassword: boolean;
}

export interface GetAvailableRolesInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
  id?: string;
}

export interface GetAvailableUsersInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
  id?: string;
}

export interface GetIdentityClaimTypesInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
}

export interface GetIdentityRoleListInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
}

export interface GetIdentitySecurityLogListInput extends ExtensiblePagedAndSortedResultRequestDto {
  startTime?: string;
  endTime?: string;
  applicationName?: string;
  identity?: string;
  action?: string;
  userName?: string;
  clientId?: string;
  correlationId?: string;
  clientIpAddress?: string;
}

export interface GetIdentitySessionListInput extends ExtensiblePagedAndSortedResultRequestDto {
  userId?: string;
  device?: string;
  clientId?: string;
}

export interface GetIdentityUserListAsFileInput extends GetIdentityUsersInput {
  token: string;
}

export interface GetIdentityUsersInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
  roleId?: string;
  organizationUnitId?: string;
  userName?: string;
  phoneNumber?: string;
  emailAddress?: string;
  name?: string;
  surname?: string;
  isLockedOut?: boolean;
  notActive?: boolean;
  emailConfirmed?: boolean;
  isExternal?: boolean;
  maxCreationTime?: string;
  minCreationTime?: string;
  maxModifitionTime?: string;
  minModifitionTime?: string;
}

export interface GetImportInvalidUsersFileInput {
  token: string;
}

export interface GetImportUsersSampleFileInput {
  fileType: ImportUsersFromFileType;
  token: string;
}

export interface GetOrganizationUnitInput extends ExtensiblePagedAndSortedResultRequestDto {
  filter?: string;
}

export interface IdentityLdapSettingsDto {
  enableLdapLogin: boolean;
  ldaps: boolean;
  ldapServerHost?: string;
  ldapServerPort?: string;
  ldapBaseDc?: string;
  ldapDomain?: string;
  ldapUserName?: string;
  ldapPassword?: string;
}

export interface IdentityLockoutSettingsDto {
  allowedForNewUsers: boolean;
  lockoutDuration: number;
  maxFailedAccessAttempts: number;
}

export interface IdentityOAuthSettingsDto {
  enableOAuthLogin: boolean;
  clientId: string;
  clientSecret?: string;
  authority: string;
  scope?: string;
  requireHttpsMetadata: boolean;
  validateEndpoints: boolean;
  validateIssuerName: boolean;
}

export interface IdentityPasswordSettingsDto {
  requiredLength: number;
  requiredUniqueChars: number;
  requireNonAlphanumeric: boolean;
  requireLowercase: boolean;
  requireUppercase: boolean;
  requireDigit: boolean;
  forceUsersToPeriodicallyChangePassword: boolean;
  passwordChangePeriodDays: number;
}

export interface IdentityRoleClaimDto {
  roleId?: string;
  claimType?: string;
  claimValue?: string;
}

export interface IdentityRoleCreateDto extends IdentityRoleCreateOrUpdateDtoBase {}

export interface IdentityRoleCreateOrUpdateDtoBase extends ExtensibleObject {
  name: string;
  isDefault: boolean;
  isPublic: boolean;
}

export interface IdentityRoleDto extends ExtensibleEntityDto<string> {
  name?: string;
  isDefault: boolean;
  isStatic: boolean;
  isPublic: boolean;
  userCount: number;
  concurrencyStamp?: string;
}

export interface IdentityRoleLookupDto extends EntityDto<string> {
  name?: string;
}

export interface IdentityRoleUpdateDto extends IdentityRoleCreateOrUpdateDtoBase {
  concurrencyStamp?: string;
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

export interface IdentitySessionSettingsDto {
  preventConcurrentLogin: IdentityProPreventConcurrentLoginBehaviour;
}

export interface IdentitySettingsDto {
  password: IdentityPasswordSettingsDto;
  lockout: IdentityLockoutSettingsDto;
  signIn: IdentitySignInSettingsDto;
  user: IdentityUserSettingsDto;
}

export interface IdentitySignInSettingsDto {
  requireConfirmedEmail: boolean;
  enablePhoneNumberConfirmation: boolean;
  requireConfirmedPhoneNumber: boolean;
}

export interface IdentityUserClaimDto {
  userId?: string;
  claimType?: string;
  claimValue?: string;
}

export interface IdentityUserCreateDto extends IdentityUserCreateOrUpdateDtoBase {
  password: string;
  sendConfirmationEmail: boolean;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
}

export interface IdentityUserCreateOrUpdateDtoBase extends ExtensibleObject {
  userName: string;
  name?: string;
  surname?: string;
  email: string;
  phoneNumber?: string;
  isActive: boolean;
  shouldChangePasswordOnNextLogin: boolean;
  lockoutEnabled: boolean;
  roleNames: string[];
  organizationUnitIds: string[];
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

export interface IdentityUserSettingsDto {
  isUserNameUpdateEnabled: boolean;
  isEmailUpdateEnabled: boolean;
}

export interface IdentityUserUpdateDto extends IdentityUserCreateOrUpdateDtoBase {
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
  concurrencyStamp?: string;
}

export interface IdentityUserUpdatePasswordInput {
  newPassword: string;
}

export interface IdentityUserUpdateRolesDto {
  roleNames: string[];
}

export interface ImportExternalUserInput {
  provider: string;
  userNameOrEmailAddress: string;
  password?: string;
}

export interface ImportUsersFromFileInputWithStream {
  file: IRemoteStreamContent;
  fileType: ImportUsersFromFileType;
}

export interface ImportUsersFromFileOutput {
  allCount: number;
  succeededCount: number;
  failedCount: number;
  invalidUsersDownloadToken?: string;
  isAllSucceeded: boolean;
}

export interface OrganizationUnitCreateDto extends OrganizationUnitCreateOrUpdateDtoBase {
  parentId?: string;
}

export interface OrganizationUnitCreateOrUpdateDtoBase extends ExtensibleObject {
  displayName: string;
}

export interface OrganizationUnitDto extends ExtensibleFullAuditedEntityDto<string> {
  parentId?: string;
  code?: string;
  displayName?: string;
  roles: OrganizationUnitRoleDto[];
}

export interface OrganizationUnitLookupDto extends EntityDto<string> {
  displayName?: string;
}

export interface OrganizationUnitMoveInput {
  newParentId?: string;
}

export interface OrganizationUnitRoleDto extends CreationAuditedEntityDto {
  organizationUnitId?: string;
  roleId?: string;
}

export interface OrganizationUnitRoleInput {
  roleIds: string[];
}

export interface OrganizationUnitUpdateDto extends OrganizationUnitCreateOrUpdateDtoBase {
  concurrencyStamp?: string;
}

export interface OrganizationUnitUserInput {
  userIds: string[];
}

export interface OrganizationUnitWithDetailsDto extends ExtensibleFullAuditedEntityDto<string> {
  parentId?: string;
  code?: string;
  displayName?: string;
  roles: IdentityRoleDto[];
  userCount: number;
  concurrencyStamp?: string;
}

export interface UpdateClaimTypeDto extends ExtensibleObject {
  name: string;
  required: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
  valueType: IdentityClaimValueType;
  concurrencyStamp?: string;
}

export interface UserLookupCountInputDto {
  filter?: string;
}

export interface UserLookupSearchInputDto extends ExtensiblePagedResultRequestDto {
  sorting?: string;
  filter?: string;
}
