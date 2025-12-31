import { escapeHtmlChars, LocalizationService } from '@abp/ng.core';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';
import { of } from 'rxjs';

export const DEFAULT_USERS_ENTITY_PROPS = EntityProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    sortable: true,
    columnWidth: 250,
    valueResolver: data => {
      const l10n = data.getInjected(LocalizationService);
      const t = l10n.instant.bind(l10n);

      const lockOutIcon = `<i title="${t(
        'AbpIdentity::ThisUserIsLockedOutMessage',
      )}" class="fas fa-lock text-danger me-1"></i>`;

      const inactiveIcon = `<i title="${t(
        'AbpIdentity::ThisUserIsNotActiveMessage',
      )}" class="fas fa-ban text-danger me-1"></i>`;

      return of(
        `
        ${data.record.isLockedOut ? lockOutIcon : ''}
        ${!data.record.isActive ? inactiveIcon : ''}
        <span class="${
          data.record.isLockedOut || !data.record.isActive ? 'opc-65' : ''
        }">${escapeHtmlChars(data.record.userName)}</span>`,
      );
    },
  },
  {
    type: ePropType.String,
    name: 'email',
    displayName: 'AbpIdentity::EmailAddress',
    sortable: true,
    columnWidth: 300,
    valueResolver: data => {
      const { email, emailConfirmed } = data.record;

      return of(
        (email || '') + (emailConfirmed ? `<i class="fa fa-check text-success ms-1"></i>` : ''),
      );
    },
  },
  {
    type: ePropType.String,
    name: 'roleNames',
    displayName: 'AbpIdentity::Roles',
    columnWidth: 250,
    valueResolver: data =>
      of(
        data.record.roleNames.reduce(
          (acc, curr) => `${acc ? acc + ',' : ''} ${escapeHtmlChars(curr)}`,
          '',
        ),
      ),
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::PhoneNumber',
    sortable: true,
    columnWidth: 250,
    valueResolver: data => {
      const { phoneNumber, phoneNumberConfirmed } = data.record;

      return of(
        (escapeHtmlChars(phoneNumber) || '') +
          (phoneNumberConfirmed ? `<i class="fa fa-check text-success ms-1"></i>` : ''),
      );
    },
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::Name',
    sortable: true,
    columnWidth: 250,
  },
  {
    type: ePropType.String,
    name: 'surname',
    displayName: 'AbpIdentity::Surname',
    sortable: true,
    columnWidth: 250,
  },
  {
    type: ePropType.Boolean,
    name: 'isActive',
    displayName: 'AbpIdentity::DisplayName:IsActive',
    sortable: true,
    columnWidth: 100,
  },
  {
    type: ePropType.Boolean,
    name: 'lockoutEnabled',
    displayName: 'AbpIdentity::DisplayName:LockoutEnabled',
    sortable: false,
    columnWidth: 100,
  },
  {
    type: ePropType.Boolean,
    name: 'emailConfirmed',
    displayName: 'AbpIdentity::DisplayName:EmailConfirmed',
    sortable: true,
    columnWidth: 100,
  },
  {
    type: ePropType.Boolean,
    name: 'twoFactorEnabled',
    displayName: 'AbpIdentity::DisplayName:TwoFactorEnabled',
    sortable: true,
    columnWidth: 100,
  },
  {
    type: ePropType.Number,
    name: 'accessFailedCount',
    displayName: 'AbpIdentity::DisplayName:AccessFailedCount',
    sortable: true,
    columnWidth: 100,
  },
  {
    type: ePropType.DateTime,
    name: 'creationTime',
    displayName: 'AbpIdentity::CreationTime',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.DateTime,
    name: 'lastModificationTime',
    displayName: 'AbpIdentity::LastModificationTime',
    sortable: true,
    columnWidth: 200,
  },
]);
