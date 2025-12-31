import { Inject, Injectable } from '@angular/core';
import {
  ABP,
  LocalizationService,
  RoutesService,
  TreeNode,
} from '@abp/ng.core';
import { LpxNavbarItem, NavbarService } from '@volo/ngx-lepton-x.core';
import { map } from 'rxjs/operators';
import {
  MobileMenuItemFilterFn,
  MOBILE_NAVBAR_ITEMS_FILTER_TOKEN,
} from '../tokens/mobile-navbar-items-filter.token';

@Injectable({
  providedIn: 'root',
})
export class AbpNavbarService {
  constructor(
    private routes: RoutesService,
    private navbarService: NavbarService,
    private localizationService: LocalizationService,
    @Inject(MOBILE_NAVBAR_ITEMS_FILTER_TOKEN)
    private mobileMenuItemFilterFn: MobileMenuItemFilterFn,
  ) {}

  private mapRouteToNavItem = (
    route: TreeNode<ABP.Route>,
    index: number,
  ): LpxNavbarItem => {
    const navbarItem: LpxNavbarItem = {
      text: this.localizationService.instant(route.name),
      link: route.children && route.children.length ? undefined : route.path,
      icon: route.iconClass,
      children: this.getRouteChildrenAsNavItems(route.children || []),
      showOnMobileNavbar: this.mobileMenuItemFilterFn(route, index),
      group: route.group,
    };

    if (route.breadcrumbText) {
      navbarItem.breadcrumbText = this.localizationService.instant(
        route.breadcrumbText,
      );
    }

    return navbarItem;
  };

  initRoutes() {
    this.routes.visible$
      .pipe(
        map((routes) =>
          routes.filter((route) => route.path || route.children.length),
        ),
      )
      .subscribe((routes) => {
        this.navbarService.setNavbarItems(
          ...routes.map(this.mapRouteToNavItem),
        );
      });
  }

  private getRouteChildrenAsNavItems(
    children: TreeNode<ABP.Route>[],
  ): LpxNavbarItem[] {
    return children.map(this.mapRouteToNavItem);
  }
}
