import { ListService } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component } from '@angular/core';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';
import { eIdentityComponents } from '../../../enums/components';
import {
  AbstractOrganizationUnitComponent,
  OrganizationUnitConfig,
  ORGANIZATION_UNIT_CONFIG,
} from '../abstract-organization-unit/abstract-organization-unit.component';
import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-organization-members',
  templateUrl: './organization-members.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.OrganizationMembers,
    },
    {
      provide: ORGANIZATION_UNIT_CONFIG,
      useValue: {
        getCurrentUnitsMethodName: 'getMembers',
        addUnitsMethodName: 'addMembers',
        addUnitsBodyPropName: 'userIds',
        deleteMethodName: 'removeMember',
        deletionLocalizationKey: 'AbpIdentity::RemoveUserFromOuWarningMessage',
      } as OrganizationUnitConfig,
    },
  ],
})
export class OrganizationMembersComponent extends AbstractOrganizationUnitComponent<IdentityUserDto> {
  modalOption: NgbModalOptions = { size: 'xl' };
}
