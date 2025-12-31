import {
  Component,
  ContentChild,
  HostBinding,
  Injector,
  TemplateRef,
  inject,
} from '@angular/core';
import { Observable } from 'rxjs';
import { ToolbarService } from '@volo/ngx-lepton-x.core';
import {
  ToolbarItem,
  ToolbarItemsDirective,
} from '../../../../components/toolbar';

@Component({
  selector: 'lpx-header-toolbar',
  templateUrl: './header-toolbar.component.html',
})
export class HeaderToolbarComponent {
  readonly #injector = inject(Injector);
  protected readonly toolbarService = inject(ToolbarService);
  
  @HostBinding('class') class = 'lpx-nav lpx-header-menu';
  @ContentChild(ToolbarItemsDirective, { read: TemplateRef })
  itemsTmp?: TemplateRef<any>;
  items$: Observable<ToolbarItem[]>;

  constructor() {
    this.items$ = this.toolbarService.items$;
  }

  hasVisibleItems(items: ToolbarItem[]) {
    const visibleItems = items.filter((item) => {
      const { visible } = item;

      let isItemVisible: boolean | Promise<boolean> | Observable<boolean> =
        true;

      if (visible) {
        isItemVisible = visible(item, this.#injector);
      }

      return isItemVisible;
    });

    return visibleItems.length > 0;
  }
}
