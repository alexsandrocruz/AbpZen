import { ConfigStateService, LocalizationService } from '@abp/ng.core';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { formatDate } from '@angular/common';
import { LOCALE_ID } from '@angular/core';
import { SaasTenantDto, TenantActivationState } from '@volo/abp.ng.saas/proxy';
import { of } from 'rxjs';

export const DEFAULT_TENANTS_ENTITY_PROPS = EntityProp.createMany<SaasTenantDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'Saas::TenantName',
    sortable: true,
    columnWidth: 300,
  },
  {
    type: ePropType.String,
    name: 'editionName',
    displayName: 'Saas::EditionName',
    columnWidth: 300,
  },
  {
    type: ePropType.DateTime,
    name: 'editionEndDateUtc',
    displayName: 'Saas::EditionEndDateUtc',
    columnWidth: 300,
    tooltip: { text: 'Saas::EditionEndDateToolTip' },
  },
  {
    type: ePropType.String,
    name: 'activationState',
    displayName: 'Saas::ActivationState',
    columnWidth: 300,
    valueResolver: data => {
      const localization = data.getInjected(LocalizationService);
      const configState = data.getInjected(ConfigStateService);
      const localeId = data.getInjected(LOCALE_ID);

      const { shortDatePattern, shortTimePattern } = configState.getDeep(
        'localization.currentCulture.dateTimeFormat',
      );

      let result = '';
      switch (data.record.activationState) {
        case TenantActivationState.Active:
          result = localization.instant('Saas::Enum:TenantActivationState.Active');
          return of(result);
        case TenantActivationState.ActiveWithLimitedTime:
          result = `${localization.instant(
            'Saas::Enum:TenantActivationState.ActiveWithLimitedTime',
          )} (${formatDate(
            data.record.activationEndDate,
            `${shortDatePattern} ${shortTimePattern.replace('tt', 'a')}`,
            localeId,
          )})`;
          return of(result);
        case TenantActivationState.Passive:
          result = localization.instant('Saas::Enum:TenantActivationState.Passive');
          return of(result);
      }
    },
  },
]);
