import { inject, Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import {
  ABP,
  LocalizationService,
  RoutesService,
  TreeNode,
} from '@abp/ng.core';
import { NavbarService } from '../navbar/navbar.service';
import { BreadcrumbItem, BreadcrumbService } from './breadcrumb.service';
import { combineLatest } from 'rxjs';
import { filter } from 'rxjs/operators';
import { LpxNavbarItem } from '../navbar';

interface BreadcrumbNavigation extends LpxNavbarItem {
  siblings: BreadcrumbNavigation[];
}

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbRouteListenerService {
  protected readonly navbarService = inject(NavbarService);
  protected readonly router = inject(Router);
  protected readonly routes = inject(RoutesService);
  protected readonly breadcrumbService = inject(BreadcrumbService);
  protected readonly localizationService = inject(LocalizationService);

  subscribeRoute(): void {
    combineLatest([
      this.router.events.pipe(
        filter((event) => event instanceof NavigationEnd),
      ),
      this.navbarService.navbarItems$.pipe(filter((items) => !!items.length)),
    ]).subscribe(([event, items]) => {
      const currentPath = (event as NavigationEnd).url;
      let activeItem = this.navbarService.findByLink(currentPath);
      let breadcrumbItems;

      if (!activeItem.item) {
        const item = this.findItemByTreeNode(currentPath);
        if (item) {
          breadcrumbItems = this.createBreadcrumbTrail(item);
          this.breadcrumbService.setItems(breadcrumbItems);
          return;
        }
        activeItem = this.navbarService.findByLink('/');
      }

      breadcrumbItems = activeItem.location.reduce((acc, itemIndex) => {
        const parent = acc[acc.length - 1]?.children || items;
        const item = parent[itemIndex];

        return [
          ...acc,
          { ...item, siblings: parent as BreadcrumbNavigation[] },
        ];
      }, [] as BreadcrumbNavigation[]);

      this.breadcrumbService.setItems(
        this.mapNavbarItemToBreadcrumbItem(breadcrumbItems),
      );
    });
  }

  private mapNavbarItemToBreadcrumbItem(
    items: BreadcrumbNavigation[],
  ): BreadcrumbItem[] {
    return items.map(({ breadcrumbText, text, link, icon, siblings }) => ({
      text: breadcrumbText || text || '',
      link,
      icon,
      children: this.mapNavbarItemToBreadcrumbItem(siblings || []),
    }));
  }

  private findItemByTreeNode(path: string): TreeNode<ABP.Route> | null {
    const { tree, search: boundSearch } = {
      tree: this.routes.tree,
      search: this.routes.search.bind(this.routes),
    };

    const treeNode = boundSearch(
      {
        path: path,
      },
      tree,
    );

    return treeNode;
  }

  private createBreadcrumbTrail(
    item: TreeNode<ABP.Route> | undefined,
  ): BreadcrumbItem[] {
    const trail: BreadcrumbItem[] = [];
    let current = item;

    while (current && current.breadcrumbText) {
      trail.push({
        text: this.localizationService.instant(current.breadcrumbText),
        icon: current.iconClass,
      });
      current = current.parent;
    }

    return trail.reverse();
  }
}
