import { escapeHtmlChars, LocalizationService } from '@abp/ng.core';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { IdentityRoleDto } from '@volo/abp.ng.identity/proxy';
import { of } from 'rxjs';

export const DEFAULT_ROLES_ENTITY_PROPS = EntityProp.createMany<IdentityRoleDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::RoleName',
    sortable: true,
    valueResolver: data => {
      const l10n = data.getInjected(LocalizationService);
      const t = l10n.instant.bind(l10n);
      const { isDefault, isPublic, name } = data.record;

      return of(
        escapeHtmlChars(name) +
          (isDefault
            ? `<span class="badge rounded-pill bg-success ms-1">${t(
                'AbpIdentity::DisplayName:IsDefault',
              )}</span>`
            : '') +
          (isPublic
            ? `<span class="badge rounded-pill bg-info ms-1">${t(
                'AbpIdentity::DisplayName:IsPublic',
              )}</span>`
            : ''),
      );
    },
  },
  {
    type: ePropType.Number,
    name: 'userCount',
    displayName: 'AbpIdentity::UserCount',
  },
]);
