import { Injectable, Injector, TemplateRef, Type } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { DataStore, sortItems } from '@volo/ngx-lepton-x.core';

export interface TopBarItem {
  action?: () => void;
  component?: Type<any>;
  injector?: Injector;
  html?: string | SafeHtml;
  id: string | number;
  order?: number;
  template?: TemplateRef<any>;
  visible?: () => boolean;
}

interface TopbarData {
  items: Array<TopBarItem>;
}

// TODO: muhammed: find out how to abstract for TobparContentService and ToolbarService
@Injectable({
  providedIn: 'root',
})
export class TopbarContentService {
  protected store = new DataStore({ items: [] } as TopbarData);

  items$ = this.store.sliceState(({ items }) => items);

  setItems(items: Array<TopBarItem>): void {
    this.store.patch({ items: items.sort(sortItems) });
  }

  addItem(item: TopBarItem): void {
    this.setItems([...this.store.state.items, item]);
  }

  patchItem(itemId: string | number, item: Omit<TopBarItem, 'id'>): void {
    const { items } = this.store.state;
    const index = items.findIndex(({ id }) => id === itemId);
    if (index === -1) {
      return;
    }
    const updateItems = [...items];
    updateItems[index] = { id: itemId, ...item };
    this.setItems(updateItems);
  }

  removeItem(id: string | number): void {
    const { items } = this.store.state;
    const index = items.findIndex((item) => item.id === id);

    if (index === -1) {
      return;
    }

    const updateItems = [...items.slice(0, index), ...items.slice(index + 1)];
    this.store.patch({ items: updateItems });
  }
}
