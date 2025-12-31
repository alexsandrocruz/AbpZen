import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import {
  OTHERS_GROUP_KEY,
  createGroupMap,
  GroupedNavbarItems,
  LpxNavbarItem,
  NavbarService,
  getItemsFromGroup,
} from '@volo/ngx-lepton-x.core';

@Injectable({
  providedIn: 'root',
})
export class MenuFilterService {
  private menuFilter$ = new BehaviorSubject('');

  get menuFilter() {
    return this.menuFilter$.value;
  }

  set menuFilter(value: string) {
    this.menuFilter$.next(value);
  }

  testAgainst = (text: string, filter: string) => {
    const regex = new RegExp('.*' + filter + '.*', 'i');
    return regex.test(text);
  };

  shouldBeExpanded = (item: LpxNavbarItem, filter: string) => {
    return !!filter || item.expanded;
  };

  filterNavbarItem = (
    items: LpxNavbarItem[] | undefined,
    filter: string,
  ): LpxNavbarItem[] => {
    if (!items || items.length === 0) {
      return [];
    }

    return items.reduce<LpxNavbarItem[]>((acc, item) => {
      const text$ = of(item.text);
      text$.pipe(take(1)).subscribe((text = '') => {
        if (item.text && this.testAgainst(text, filter)) {
          acc.push({ ...item, expanded: this.shouldBeExpanded(item, filter) });
        } else {
          const children = this.filterNavbarItem(item.children, filter);
          if (children && children.length) {
            acc.push({
              ...item,
              children: children,
              expanded: this.shouldBeExpanded(item, filter),
            });
          }
        }
      });
      return acc;
    }, []);
  };

  constructor(private navbarService: NavbarService) {
    this.addFilterToNavItems$();
  }

  addFilterToNavItems$() {
    const groupFilter$ = this.navbarService.groupedNavbarItems$.pipe(
      switchMap((groupedItems) =>
        this.menuFilter$.pipe(
          map((filter) => {
            groupedItems ??= [];
            const items = getItemsFromGroup<GroupedNavbarItems, LpxNavbarItem>(
              groupedItems,
            );

            if (!items) {
              return [];
            }

            let list: LpxNavbarItem[] = [];

            if (filter) {
              list = this.filterNavbarItem(items, filter);
            } else {
              const { location } = this.navbarService.getRouteItem();
              list = this.navbarService.calculateExpandState(items, location);
            }

            const map = createGroupMap(list, OTHERS_GROUP_KEY, !!filter);
            if (!map) {
              return [];
            }

            return Array.from(map, ([group, items]) => ({
              group,
              items,
            }));
          }),
        ),
      ),
    );

    this.navbarService.groupedNavbarItems$ = groupFilter$;
    this.navbarService.navbarItems$ = this.navbarService.navbarItems$.pipe(
      switchMap((items) =>
        this.menuFilter$.pipe(
          map((filter) => {
            if (filter) {
              return this.filterNavbarItem(items, filter);
            }
            const route = this.navbarService.getRouteItem();
            return this.navbarService.calculateExpandState(
              items,
              route.location,
            );
          }),
        ),
      ),
    );
  }

  clearFilter(): void {
    this.menuFilter = '';
  }
}
