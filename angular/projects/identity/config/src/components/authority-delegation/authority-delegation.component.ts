import { Component, TrackByFunction } from '@angular/core';
import { ABP } from '@abp/ng.core';
import { ListAuthorityDelegationComponent } from './list-authority-delegation.component';
import { MyDelegatedUsersComponent } from './my-delegated-users.component';

@Component({
  selector: 'abp-authority-delegation',
  templateUrl: './authority-delegation.component.html',
})
export class AuthorityDelegationComponent {
  isModalOpen = true;
  option = { size: 'lg' };

  tabs: ABP.Tab[] = [
    {
      name: 'AbpIdentity::DelegatedUsers',
      component: ListAuthorityDelegationComponent,
    },
    {
      name: 'AbpIdentity::MyDelegatedUsers',
      component: MyDelegatedUsersComponent,
    },
  ];
  selected: ABP.Tab = this.tabs[0];
  trackByFn: TrackByFunction<ABP.Tab> = (_, item) => item.name;
}
