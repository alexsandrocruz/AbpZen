import { Injectable } from '@angular/core';
import { DataStore } from '../../utils/data-store';
import { Observable } from 'rxjs';

// TODO : add onClick
export interface BreadcrumbItem {
  icon?: string;
  text: string | Observable<string>;
  link?: string;
  children?: BreadcrumbItem[];
  expanded?: boolean;
  active?: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbService {
  private store = new DataStore<BreadcrumbItem[]>([]);
  items$ = this.store.sliceState((state) => state);
  // TODO: generate id per item
  add(item: BreadcrumbItem | BreadcrumbItem[]): void {
    const items = Array.isArray(item) ? item : [item];
    this.store.set([...this.store.state, ...items]);
  }

  // TODO: generate id per item
  insert(item: BreadcrumbItem | BreadcrumbItem[], index: number): void {
    const state = this.store.state;
    const items = Array.isArray(item) ? item : [item];
    this.store.set([...state.slice(0, index), ...items, ...state.slice(index)]);
  }

  // TODO: generate id per item
  setItems(items: BreadcrumbItem[]): void {
    this.store.set(items);
  }

  // TODO: Add remove function
}
