import { Injectable } from '@angular/core';
import { LpxNavbarItem } from '@volo/ngx-lepton-x.core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TopMenuSubNavbarService {
  private _items$ = new BehaviorSubject<LpxNavbarItem[]>([]);
  public readonly items$ = this._items$.asObservable();

  setItems(value: LpxNavbarItem[]) {
    this._items$.next(value);
  }
}
