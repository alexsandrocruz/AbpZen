import { MOBILE_NAVBAR_ITEMS_FILTER_TOKEN } from './../tokens/mobile-navbar-items-filter.token';
import { ABP, TreeNode } from '@abp/ng.core';
import { MobileMenuItemFilterFn } from '../tokens/mobile-navbar-items-filter.token';

export const MOBILE_NAVBAR_ITEMS_FILTER_PROVIDER = {
  provide: MOBILE_NAVBAR_ITEMS_FILTER_TOKEN,
  deps: [],
  useFactory: mobileMenuItemFilterFnFactory
}

export function mobileMenuItemFilterFnFactory() :MobileMenuItemFilterFn {
  return (route: TreeNode<ABP.Route>,
    index: number) => {
    return index === 0 || index === 1
  }
}
