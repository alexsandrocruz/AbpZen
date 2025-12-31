import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { transition, trigger, useAnimation } from '@angular/animations';
import { switchMap, tap } from 'rxjs';
import { ABP, ConfigStateService, SubscriptionService, TrackByService } from '@abp/ng.core';
import { fadeIn } from '@abp/ng.theme.shared';
import { twoFactorBehaviourOptions } from '@volo/abp.ng.account/admin';
import {
  eAccountManageProfileTabNames,
  ManageProfileTabsService,
} from '@volo/abp.ng.account/public/config';
import { ProfileDto, ProfileService } from '@volo/abp.ng.account/public/proxy';
import { ManageProfileStateService } from '../../services/manage-profile-state.service';

@Component({
  selector: 'abp-manage-profile',
  templateUrl: './manage-profile.component.html',
  animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ManageProfileComponent {
  protected readonly subscriptionService = inject(SubscriptionService);

  firstTab: ABP.Tab;
  selected: ABP.Tab;
  isProfileLoaded: boolean;

  tabs$ = this.tabsService.visible$.pipe(
    tap(tabs => {
      this.firstTab = tabs[0];
      if (!this.selected && tabs[0].component) {
        this.selected = tabs[0];
      }
    }),
  );

  get isTwoFactorEnabled(): boolean {
    const { key } = twoFactorBehaviourOptions[0];
    return (
      this.configState.getFeature('Identity.TwoFactor') === key &&
      this.configState.getSetting('Abp.Identity.TwoFactor.Behaviour') === key &&
      (
        (this.configState.getSetting('Abp.Identity.TwoFactor.UsersCanChange') as string) || ''
      ).toLowerCase() === 'true'
    );
  }

  protected setProfile(profile: ProfileDto): void {
    this.manageProfileState.setProfile(profile as ProfileDto);
    this.isProfileLoaded = true;
    if (profile.isExternal) {
      this.tabsService.patch(eAccountManageProfileTabNames.ChangePassword, {
        invisible: true,
      });
      this.selected = this.firstTab;
    }
  }

  protected patchTwoFactorTab(canEnableTwoFactor: boolean): void {
    this.tabsService.patch(eAccountManageProfileTabNames.TwoFactor, {
      invisible: !this.isTwoFactorEnabled || !canEnableTwoFactor,
    });
  }

  protected init(): void {
    const profile$ = this.profileService.get().pipe(
      tap(profile => this.setProfile(profile)),
      switchMap(() => this.profileService.canEnableTwoFactor()),
      tap(canEnable => this.patchTwoFactorTab(canEnable)),
    );

    this.subscriptionService.addOne(profile$);
  }

  constructor(
    public readonly track: TrackByService,
    private tabsService: ManageProfileTabsService,
    private configState: ConfigStateService,
    protected profileService: ProfileService,
    protected manageProfileState: ManageProfileStateService,
  ) {
    this.init();
  }
}
