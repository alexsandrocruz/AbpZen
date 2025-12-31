import { Injectable, inject } from '@angular/core';
import { NavItem, NavItemsService } from '@abp/ng.theme.shared';
import { switchMap } from 'rxjs/operators';
import { PermissionService } from '@abp/ng.core';
import { ToolbarService, ToolbarItem } from '@volo/ngx-lepton-x.core';

@Injectable({
  providedIn: 'root',
})
export class AbpToolbarService {
  protected readonly toolbar = inject(ToolbarService);
  protected readonly navItems = inject(NavItemsService);
  protected readonly permissionService = inject(PermissionService);

  listenNavItems() {
    this.navItems.items$
      .pipe(
        switchMap((items: NavItem[]) =>
          this.permissionService.filterItemsByPolicy$(items),
        ),
      )
      .subscribe((items) => this.toolbar.setItems(items as ToolbarItem[]));
  }
}
