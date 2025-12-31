import { Observable } from 'rxjs';
import { TemplateRef, Type, Injector } from '@angular/core';

export interface GroupedNavbarItems {
  group: string;
  items: LpxNavbarItem[];
}
export interface LpxNavbarItem {
  action?: NavBarPropPredicate<LpxNavbarItem>;
  children?: LpxNavbarItem[];
  containerLink?: string;
  component?: Type<any>;
  expanded?: boolean;
  icon?: string;
  id?: string;
  link?: string;
  selected?: boolean;
  showOnMobileNavbar?: boolean;
  template?: TemplateRef<any>;
  text?: string;
  breadcrumbText?: string;
  visible?: NavBarPropPredicate<LpxNavbarItem>;
  group?: string;
}

export type NavBarPropPredicate<T> = (
  prop?: T,
  injector?: Injector,
) => boolean | Promise<boolean> | Observable<boolean>;
