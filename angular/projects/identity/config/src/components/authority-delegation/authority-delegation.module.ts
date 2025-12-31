import { NgModule } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgbDropdownModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AuthorityDelegationComponent } from './authority-delegation.component';
import { ListAuthorityDelegationComponent } from './list-authority-delegation.component';
import { MyDelegatedUsersComponent } from './my-delegated-users.component';
import { CreateUserDelegateComponent } from './create-user-delegate.component';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

const declarationsWithExports = [
  AuthorityDelegationComponent,
  ListAuthorityDelegationComponent,
  MyDelegatedUsersComponent,
];

@NgModule({
  declarations: [...declarationsWithExports],
  exports: [...declarationsWithExports],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgbNavModule,
    NgxValidateCoreModule,
    CommercialUiModule,
    CreateUserDelegateComponent,
  ],
})
export class AuthorityDelegationModule {}
