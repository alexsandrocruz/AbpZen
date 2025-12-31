import { Injectable } from '@angular/core';
import { DataStore } from '../../utils/data-store';
import { UserProfile } from '../../models';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  private store = new DataStore({} as UserProfile);
  user$ = this.store.sliceState((state) => state);

  setUser(user: UserProfile) {
    this.store.set(user);
  }

  patchUser(user: Partial<UserProfile>) {
    this.store.patch(user);
  }
}
