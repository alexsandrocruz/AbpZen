import { getShortDateShortTimeFormat, ConfigStateService } from '@abp/ng.core';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { DatePipe } from '@angular/common';
import { IdentitySecurityLogDto } from '@volo/abp.commercial.ng.ui/config';
import { of } from 'rxjs';
import { MySecurityLogsComponent } from '../components/my-security-logs/my-security-logs.component';

export const DEFAULT_MY_SECURITY_LOGS_ENTITY_PROPS = EntityProp.createMany<IdentitySecurityLogDto>([
  {
    type: ePropType.String,
    name: 'creationTime',
    displayName: 'AbpAccount::MySecurityLogs:Time',
    sortable: true,
    columnWidth: 180,
    valueResolver: data => {
      const config = data.getInjected(ConfigStateService);
      const datePipe = data.getInjected(DatePipe);

      return of(datePipe.transform(data.record.creationTime, getShortDateShortTimeFormat(config)));
    },
  },
  {
    type: ePropType.String,
    name: 'action',
    displayName: 'AbpAccount::MySecurityLogs:Action',
    sortable: true,
    columnWidth: 180,
    valueResolver: data => {
      return of(
        `<div style="max-width: 160px" class="pointer abp-ellipsis-inline">${
          data.record.action || ''
        }</div>`,
      );
    },
    action: data => {
      const component = data.getInjected(MySecurityLogsComponent);
      component.filter.action = data.record.action;
      component.list.get();
    },
  },
  {
    type: ePropType.String,
    name: 'clientIpAddress',
    displayName: 'AbpAccount::MySecurityLogs:IpAddress',
    sortable: false,
    columnWidth: 200,
    valueResolver: data => {
      return of(
        `<div style="max-width: 180px" class="abp-ellipsis-inline">${
          data.record.clientIpAddress || ''
        }</div>`,
      );
    },
  },
  {
    type: ePropType.String,
    name: 'browserInfo',
    displayName: 'AbpAccount::MySecurityLogs:Browser',
    sortable: false,
    columnWidth: 200,
    valueResolver: data => {
      return of(
        `<div style="max-width: 180px" class="abp-ellipsis-inline">${
          data.record.browserInfo || ''
        }</div>`,
      );
    },
  },
  {
    type: ePropType.String,
    name: 'applicationName',
    displayName: 'AbpAccount::MySecurityLogs:Application',
    sortable: true,
    columnWidth: 150,
    valueResolver: data => {
      return of(
        `<div style="max-width: 135px" class="abp-ellipsis-inline">${
          data.record.applicationName || ''
        }</div>`,
      );
    },
  },
  {
    type: ePropType.String,
    name: 'identity',
    displayName: 'AbpAccount::MySecurityLogs:Identity',
    sortable: true,
    columnWidth: 150,
    valueResolver: data => {
      return of(
        `<div style="max-width: 135px" class="abp-ellipsis-inline">${
          data.record.identity || ''
        }</div>`,
      );
    },
  },
  {
    type: ePropType.String,
    name: 'clientId',
    displayName: 'AbpAccount::MySecurityLogs:Client',
    sortable: true,
    columnWidth: 320,
    valueResolver: data => {
      return of(`<div>${data.record.clientId || ''}</div>`);
    },
  },
]);
