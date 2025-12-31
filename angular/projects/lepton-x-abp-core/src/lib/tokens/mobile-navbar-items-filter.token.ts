import { ABP, TreeNode } from '@abp/ng.core';
import { InjectionToken } from '@angular/core';

export type MobileMenuItemFilterFn = (
  route: TreeNode<ABP.Route>,
  index: number
) => boolean;
export const MOBILE_NAVBAR_ITEMS_FILTER_TOKEN =
  new InjectionToken<MobileMenuItemFilterFn>(
    'MOBILE_NAVBAR_ITEMS_FILTER_TOKEN'
  );
