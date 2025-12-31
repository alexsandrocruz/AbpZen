import { InternalStore } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { ProfileDto } from '@volo/abp.ng.account/public/proxy';
import { Observable } from 'rxjs';

export interface ManageProfileState {
  profile: ProfileDto;
  hideEmailVerificationBtn: boolean;
}

@Injectable({ providedIn: 'root' })
export class ManageProfileStateService {
  private readonly store = new InternalStore({} as ManageProfileState);

  get createStateStream() {
    return this.store.sliceState;
  }

  getProfile$(): Observable<ProfileDto> {
    return this.store.sliceState(state => state.profile);
  }

  getProfile(): ProfileDto {
    return this.store.state.profile;
  }

  setProfile(profile: ProfileDto) {
    this.store.patch({ profile });
  }

  setHideEmailVerificationBtn(hideEmailVerificationBtn: boolean) {
    this.store.patch({ hideEmailVerificationBtn });
  }
}
