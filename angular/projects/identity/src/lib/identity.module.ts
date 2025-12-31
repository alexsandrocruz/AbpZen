/* tslint:disable:max-line-length */
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import {
  NgbTooltipModule,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbNavModule,
} from '@ng-bootstrap/ng-bootstrap';
import { PageModule } from '@abp/ng.components/page';
import { TreeModule } from '@abp/ng.components/tree';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  AdvancedEntityFiltersModule,
  CommercialUiModule,
  DatetimePickerComponent,
} from '@volo/abp.commercial.ng.ui';

import { IdentityConfigOptions } from './models/config-options';
import { IdentityRoutingModule } from './identity-routing.module';
import {
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';

/*
  TODO: group imports by domain name but don't forget to export by folder with index.ts, e.g:
  - identity
    import { UserLockModalComponent, SetUserPasswordComponent, UserSessionsComponent} from './components/users';
  - roles
    import { RolesComponent, MoveAllUsersComponent, RoleDeleteComponent } from './components/roles';
*/
import { RolesComponent } from './components/roles/roles.component';
import { MoveAllUsersComponent } from './components/roles/move-all-users/move-all-users.component';
import { RoleDeleteComponent } from './components/roles/delete-role/role-delete.component';
import { RolesModalComponent } from './components/roles/edit-new-modal/roles-modal.component';

import { UsersComponent } from './components/users/users.component';
import { UserLockModalComponent } from './components/users/user-lock-modal.component';
import { SetUserPasswordComponent } from './components/users/set-password-modal/set-password-modal.component';
import { UserSessionsComponent } from './components/users/user-sessions-modal/user-sessions.component';
import { UserViewDetailsComponent } from './components/users/user-view-details/user-view-details.component';

import { DeleteOrganizationUnitComponent } from './components/organization-units/delete-organization-unit/delete-organization-unit.component';
import { MoveAllUsersOfUnitComponent } from './components/organization-units/move-all-users-of-unit/move-all-users-of-unit.component';
import { AbstractOrganizationUnitComponent } from './components/organization-units/abstract-organization-unit/abstract-organization-unit.component';
import { OrganizationMembersModalBodyComponent } from './components/organization-units/organization-members/organization-members-modal-body.component';
import { OrganizationMembersComponent } from './components/organization-units/organization-members/organization-members.component';
import { OrganizationRolesModalBodyComponent } from './components/organization-units/organization-roles/organization-roles-modal-body.component';
import { OrganizationRolesComponent } from './components/organization-units/organization-roles/organization-roles.component';
import { OrganizationUnitsComponent } from './components/organization-units/organization-units.component';
import { SelectedOrganizationUnitComponent } from './components/organization-units/selected-organization-unit/selected-organization-unit.component';

import { ExternalLoginComponent } from './components/external-login/external-login.component';
import { UserDropdownMenuComponent } from './components/external-login/user-dropdown-menu/user-dropdown-menu.component';
import { ExportDataDropdownComponent } from './components/export-excel/export-data-dropdown.component';
import { UploadExcelFileComponent } from './components/export-excel/upload-excel-file.component';

import { SecurityLogsComponent } from './components/security-logs/security-logs.component';

import { ClaimModalComponent } from './components/claim-modal/claim-modal.component';
import { ClaimsComponent } from './components/claims/claims.component';

const exportAndDeclarations = [
  UsersComponent,
  OrganizationUnitsComponent,
  OrganizationMembersComponent,
  OrganizationMembersModalBodyComponent,
  OrganizationRolesComponent,
  OrganizationRolesModalBodyComponent,
  AbstractOrganizationUnitComponent,
  SecurityLogsComponent,
  SelectedOrganizationUnitComponent,
  ExternalLoginComponent,
  UserDropdownMenuComponent,
  RolesComponent,
  MoveAllUsersComponent,
  RoleDeleteComponent,
  ExportDataDropdownComponent,
  ClaimsComponent,
  ClaimModalComponent,
];

@NgModule({
  declarations: [...exportAndDeclarations],
  exports: [...exportAndDeclarations],
  imports: [
    CoreModule,
    CommercialUiModule,
    IdentityRoutingModule,
    NgbNavModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgbTooltipModule,
    NgbDatepickerModule,
    PermissionManagementModule,
    NgxValidateCoreModule,
    TreeModule,
    PageModule,
    AdvancedEntityFiltersModule,
    ExtensibleModule,
    UserViewDetailsComponent,
    DeleteOrganizationUnitComponent,
    MoveAllUsersOfUnitComponent,
    UploadExcelFileComponent,
    RolesModalComponent,
    UserLockModalComponent,
    SetUserPasswordComponent,
    UserSessionsComponent,
    DatetimePickerComponent,
  ],
})
export class IdentityModule {
  static forChild(options: IdentityConfigOptions = {}): ModuleWithProviders<IdentityModule> {
    return {
      ngModule: IdentityModule,
      providers: [
        {
          provide: IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: IDENTITY_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
      ],
    };
  }

  static forLazy(options: IdentityConfigOptions = {}): NgModuleFactory<IdentityModule> {
    return new LazyModuleFactory(IdentityModule.forChild(options));
  }
}
