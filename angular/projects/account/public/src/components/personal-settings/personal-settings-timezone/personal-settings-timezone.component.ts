import { Component, inject } from '@angular/core';
import {
  EXTENSIBLE_FORM_VIEW_PROVIDER,
  EXTENSIONS_FORM_PROP,
  EXTENSIONS_FORM_PROP_DATA,
} from '@abp/ng.components/extensible';
import { ProfileService, Volo } from '@volo/abp.ng.account/public/proxy';
import { ConfigStateService, SubscriptionService } from '@abp/ng.core';
import { TrackByService } from '@abp/ng.core';
import { Observable } from 'rxjs';

@Component({
  selector: 'abp-personal-settings-time-zone',
  templateUrl: './personal-settings-timezone.component.html',
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
})
export class PersonalSettingsTimerZoneComponent {
  protected readonly profileService = inject(ProfileService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly formProp = inject(EXTENSIONS_FORM_PROP);
  protected readonly propData = inject(EXTENSIONS_FORM_PROP_DATA);
  protected readonly track = inject(TrackByService);
  timeZones$: Observable<Volo.Abp.NameValue[]> = this.profileService.getTimezones();

  get name() {
    return this.formProp.name;
  }
}
