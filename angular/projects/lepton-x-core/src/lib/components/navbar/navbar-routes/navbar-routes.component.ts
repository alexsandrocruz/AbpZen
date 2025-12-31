import {
  Component,
  effect,
  EventEmitter,
  inject,
  Injector,
  input,
  Input,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { GroupedNavbarItems, LpxNavbarItem } from '../models';
import { getItemsFromGroup } from '../../../utils';
import { RoutesService } from '../../../services/route/routes.service';

export type NavbarItemsType = LpxNavbarItem[] | null | undefined;
export type NavbarGroupItemsType = GroupedNavbarItems[] | null | undefined;

@Component({
  selector: 'lpx-navbar-routes',
  templateUrl: './navbar-routes.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class NavbarRoutesComponent {
  protected readonly injector = inject(Injector);
  protected readonly routesService = inject(RoutesService);

  @Input()
  groupedItems: NavbarGroupItemsType;

  @Input()
  navbarItems: NavbarItemsType;

  routerItem = input<boolean>();

  @Output()
  routeClick = new EventEmitter<LpxNavbarItem>();

  private get itemsFromGroup(): LpxNavbarItem[] | undefined {
    if (!this.groupedItems) {
      return undefined;
    }

    return getItemsFromGroup<GroupedNavbarItems, LpxNavbarItem>(
      this.groupedItems,
    );
  }

  constructor() {
    this.fixNavbarItemsByRouter();
  }

  private isExpandedOrSelected = (item: LpxNavbarItem): boolean =>
    !!(item.expanded || item.selected);

  onSubnavbarExpand(menuItem: LpxNavbarItem, menuItems: NavbarItemsType): void {
    if (menuItem.expanded) {
      const items = this.itemsFromGroup || menuItems;
      if (!items) {
        return;
      }

      items
        .filter((item) => item !== menuItem)
        .forEach((item) => (item.expanded = false));
    }
  }

  onRouteClick(menuItem: LpxNavbarItem, menuItems: NavbarItemsType): void {
    const expandedItems = menuItems?.filter(this.isExpandedOrSelected);
    const expandedGroupItems = this.itemsFromGroup?.filter(
      this.isExpandedOrSelected,
    );

    const items = expandedGroupItems || expandedItems;

    if (items) {
      items
        .filter((item) => item !== menuItem)
        .reduce<LpxNavbarItem[]>((acc, item) => {
          return [...acc, item, ...this.flatChildren(item.children || [])];
        }, [])
        ?.filter(
          (item) =>
            !this.checkChildrenIncludesItem(item, menuItem) &&
            item !== menuItem,
        )
        .forEach((item) => {
          item.selected = false;
          item.expanded = false;
        });
    }

    this.routeClick.emit(menuItem);
  }

  checkChildrenIncludesItem(
    item: LpxNavbarItem,
    menuItem: LpxNavbarItem,
  ): boolean {
    return (
      item.children?.reduce(
        (acc, child) =>
          acc ||
          child === menuItem ||
          this.checkChildrenIncludesItem(child, menuItem),
        false,
      ) || false
    );
  }

  flatChildren(menuItems: NavbarItemsType): LpxNavbarItem[] {
    return (
      menuItems?.reduce<LpxNavbarItem[]>((acc, item) => {
        return [...acc, item, ...this.flatChildren(item.children || [])];
      }, []) || []
    );
  }

  fixNavbarItemsByRouter() {
    effect(() => {
      const currentNavigation = this.routesService.currentNavigation();

      if (!currentNavigation) {
        return;
      }

      this.fixNavbarItems(
        currentNavigation,
        this.navbarItems as LpxNavbarItem[],
      );
    });
  }

  fixNavbarItems(currentUrl: string, items: LpxNavbarItem[]): void {
    if (!items) {
      return;
    }

    for (const item of items) {
      if (item.children?.length) {
        item.expanded = this.hasUrlInChildren(item, currentUrl);
        this.fixNavbarItems(currentUrl, item.children);
      } else {
        item.selected = item.link === currentUrl;
      }
    }
  }

  hasUrlInChildren(item: LpxNavbarItem, url: string): boolean {
    if (item.link === url) {
      return true;
    }

    if (item.children) {
      for (const child of item.children) {
        const found = this.hasUrlInChildren(child, url);
        if (found) {
          return true;
        }
      }
    }

    return false;
  }
}
