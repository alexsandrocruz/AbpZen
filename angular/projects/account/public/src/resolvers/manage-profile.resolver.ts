import { ABP, ReplaceableComponentsService, TreeNode } from '@abp/ng.core';
import { Injectable, Type } from '@angular/core';
import {
  eAccountManageProfileTabNames,
  ManageProfileTabsService,
} from '@volo/abp.ng.account/public/config';
import { take, tap } from 'rxjs/operators';
import { ChangePasswordComponent } from '../components/change-password/change-password.component';
import { PersonalSettingsComponent } from '../components/personal-settings/personal-settings.component';
import { ProfilePictureComponent } from '../components/profile-picture/profile-picture.component';
import { TwoFactorTabComponent } from '../components/two-factor-tab/two-factor-tab.component';
import { AuthenticatorAppComponent } from '../components';

@Injectable()
export class ManageProfileResolver {
  constructor(
    private manageProfileTabs: ManageProfileTabsService,
    private replaceableComponentsService: ReplaceableComponentsService,
  ) {}

  resolve() {
    return this.manageProfileTabs.visible$.pipe(tap(this.addComponentsToTabs), take(1));
  }

  private addComponentsToTabs = (tabs: TreeNode<Omit<ABP.Tab, 'parentName'>>[]) => {
    tabs.forEach(tab => {
      if (tab.component) return;

      const replaceableComponent = this.replaceableComponentsService.get(tab.name);
      const component: Type<any> = replaceableComponent
        ? replaceableComponent.component
        : ComponentList[tab.name];
      if (!component) {
        return;
      }
      this.manageProfileTabs.patch(tab.name, { component });
    });
  };
}

const ComponentList = {
  [eAccountManageProfileTabNames.ProfilePicture]: ProfilePictureComponent,
  [eAccountManageProfileTabNames.ChangePassword]: ChangePasswordComponent,
  [eAccountManageProfileTabNames.PersonalInfo]: PersonalSettingsComponent,
  [eAccountManageProfileTabNames.AuthenticatorApp]: AuthenticatorAppComponent,
  [eAccountManageProfileTabNames.TwoFactor]: TwoFactorTabComponent,
};
