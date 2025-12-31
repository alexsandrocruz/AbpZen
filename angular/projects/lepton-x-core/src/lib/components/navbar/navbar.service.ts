import { Injectable, inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map, take, tap } from 'rxjs/operators';
import { DataStore } from '../../utils/data-store';
import { LpxNavbarItem } from './models';
import { LPX_MENU_ITEMS } from './navbar.token';
import { createGroupMap } from '../../utils';
import { OTHERS_GROUP_KEY } from '../../consts';

@Injectable({
  providedIn: 'root',
})
export class NavbarService {
  protected readonly router = inject(Router);
  menuItems = inject(LPX_MENU_ITEMS);

  private store = new DataStore<LpxNavbarItem[]>(
    this.addContainerLinks(this.menuItems),
  );

  navbarItems$ = this.store.sliceState((state) => state);

  groupedNavbarItems$ = this.store
    .sliceState((state) => state)
    .pipe(
      map((items) => {
        if (!items.some((f) => !!f.group)) {
          return;
        }

        const map = createGroupMap(items, OTHERS_GROUP_KEY) || [];
        return Array.from(map, ([group, items]) => ({
          group,
          items,
        }));
      }),
    );

  constructor() {
    this.expandItemByLink$().pipe(take(1)).subscribe();
  }

  addNavbarItems(...menuItems: LpxNavbarItem[]): void {
    this.store.set([...this.store.state, ...this.addContainerLinks(menuItems)]);
  }

  setNavbarItems(...menuItems: LpxNavbarItem[]): void {
    this.store.set([...this.addContainerLinks(menuItems)]);
  }

  // TODO: muhammed: refactor this method to be readable
  addChildren(id: string, ...menuItems: LpxNavbarItem[]): void {
    const parent = this.findById(id, this.store.state);
    const update = (
      items: LpxNavbarItem[],
      location: Array<number>,
      link = '',
    ): LpxNavbarItem[] => {
      const i = location.shift();
      return items.reduce((acc, item, index) => {
        return [
          ...acc,
          ...(index === i
            ? [
                {
                  ...item,
                  children: !location.length
                    ? [
                        ...(item.children || []),
                        ...this.addContainerLinks(
                          menuItems,
                          `${link}/${item.containerLink}`,
                        ),
                      ]
                    : update(
                        item.children || [],
                        location,
                        `${link}/${item.containerLink}`,
                      ),
                },
              ]
            : [item]),
        ];
      }, [] as LpxNavbarItem[]);
    };

    const updated = update(this.store.state, parent.location);
    this.store.patch(updated);
  }

  findByLink(link: string, items?: LpxNavbarItem[]) {
    return this.findByProp('link', link, items);
  }

  expandItemByLink$() {
    return this.router.events.pipe(
      filter((e) => e instanceof NavigationEnd),
      tap(() => this.expandItems()),
    );
  }

  expandItems() {
    const route = this.getRouteItem();
    if (route?.item) {
      const expanded = this.calculateExpandState(
        this.store.state,
        route.location,
      );
      this.store.patch(expanded);
    }
  }

  getRouteItem() {
    return this.findByLink(this.router.url);
  }

  calculateExpandState(
    items: LpxNavbarItem[],
    indexes: Array<number>,
  ): LpxNavbarItem[] {
    const matchIndex = indexes.shift();
    return items.reduce((acc, item, index) => {
      if (index === matchIndex) {
        return [
          ...acc,
          {
            ...item,
            expanded: true,
            selected: true,
            children: this.calculateExpandState(item.children || [], indexes),
          },
        ];
      }
      const newItem = {
        ...item,
        ...(item.children
          ? { children: this.collapseChildren(item.children) }
          : {}),
      };
      return [...acc, { ...newItem, expanded: false, selected: false }];
    }, [] as LpxNavbarItem[]);
  }

  collapseChildren(children: Array<LpxNavbarItem>): Array<LpxNavbarItem> {
    return [
      ...children.map((child) => ({
        ...child,
        expanded: false,
        selected: false,
        children: child.children ? this.collapseChildren(child.children) : [],
      })),
    ];
  }

  private findById(id: string, items?: LpxNavbarItem[]) {
    return this.findByProp('id', id, items);
  }

  private findByProp(
    prop: keyof LpxNavbarItem,
    value: string,
    items?: LpxNavbarItem[],
    location: Array<number> = [],
  ) {
    const navbarItems = items || this.store.state;
    const itemIndex = navbarItems.findIndex((i) => i[prop] === value);
    let item;
    if (itemIndex === -1) {
      navbarItems.forEach((i, index) => {
        if (i.children) {
          const child = this.findByProp(prop, value, i.children, [
            ...location,
            index,
          ]);
          if (child?.item) {
            item = child.item;
            location = child.location;
          }
        }
      });
    } else {
      item = navbarItems[itemIndex];
      location.push(itemIndex);
    }

    return { item, location };
  }

  private addContainerLinks(
    items: LpxNavbarItem[],
    link = '',
  ): LpxNavbarItem[] {
    return items.map((item) => ({
      ...item,
      ...(item.link && link ? { link: `${link}/${item.link}` } : {}),
      children: this.addContainerLinks(
        item.children || [],
        `${link ? link + '/' : ''}${item.containerLink || ''}`,
      ),
    }));
  }
}
