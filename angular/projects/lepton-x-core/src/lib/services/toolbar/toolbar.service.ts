import { Injectable } from '@angular/core';
import { ToolbarItem } from '../../models/toolbar-item.model';
import { DataStore, sortItems } from '../../utils';

@Injectable({ providedIn: 'root' })
export class ToolbarService {
  private store = new DataStore({ items: <Array<ToolbarItem>>[] });

  items$ = this.store.sliceState(({ items }) => items);

  setItems(items: Array<ToolbarItem>): void {
    this.store.patch({ items: items.sort(sortItems) });
  }

  addItem(item: ToolbarItem): void {
    this.setItems([...this.store.state.items, item]);
  }

  patchItem(itemId: string | number, item: Omit<ToolbarItem, 'id'>): void {
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
