import { Component } from '@angular/core';
import { OrganizationUnitsStateService } from '../../../services/organization-units-state.service';

@Component({
  selector: 'abp-selected-organization-unit',
  templateUrl: './selected-organization-unit.component.html',
})
export class SelectedOrganizationUnitComponent {
  constructor(public organizationUnitsStateService: OrganizationUnitsStateService) {}
}
