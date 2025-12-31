import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LpxNavbarItem } from '@volo/ngx-lepton-x.core';

export function combineAndFilterByChildren<
  T extends { children: LpxNavbarItem[] }
>(obs: Observable<T>[]) {
  return combineLatest(obs).pipe(
    map((parents) => parents.filter((parent) => parent.children.length > 0))
  );
}
