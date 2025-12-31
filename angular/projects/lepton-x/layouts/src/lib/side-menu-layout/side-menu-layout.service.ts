import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import {
  DataStore,
  LayoutService,
  LpxNavbarItem,
} from '@volo/ngx-lepton-x.core';
import { LPX_CONTAINERS } from '../tokens';
import { LpxContainer } from '../models';
import { SideMenuLayoutTranslate } from './enums';
import { LpxLocalStorageService } from '@volo/ngx-lepton-x.core';
@Injectable({
  providedIn: 'root',
})
export class SideMenuLayoutService {
  readonly store = new DataStore({
    layouts: [] as LpxContainer[],
    selectedLayout: {} as LpxContainer,
  });

  private containerWidthTypes: string[] = [];

  id = 'layouts';

  convertLayoutToNavbarItem = (layouts: LpxContainer[]): LpxNavbarItem[] => {
    return layouts.map((layout) => ({
      icon: layout.icon,
      text: layout.label,
      selected: layout.default,
      action: () => {
        this.changeLayout(layout);
        return true;
      },
    }));
  };

  selectedLayout$ = this.store.sliceState((state) => state.selectedLayout);
  containerClass$ = this.layoutService.containerClass$;
  layouts$ = this.store.sliceState((state) => state.layouts);
  layoutsAsNavbarItems$ = this.layouts$.pipe(
    map(this.convertLayoutToNavbarItem)
  );
  layoutsAsSettingsGroup$ = this.layoutsAsNavbarItems$.pipe(
    // TODO: PROVIDE API
    map((layouts) => ({
      text: SideMenuLayoutTranslate.SettingsTitle,
      icon: 'bi bi-aspect-ratio',
      id: this.id,
      children: layouts,
    }))
  );

  constructor(
    @Inject(LPX_CONTAINERS) private containers: LpxContainer[],
    private layoutService: LayoutService,
    private localStorageService: LpxLocalStorageService
  ) {
    this.initLayout();
  }

  initLayout(): void {
    this.store.patch({ layouts: this.containers });
    this.containerWidthTypes = this.containers.map(
      (container) => container.layoutClass
    );
    const defaultLayout = this.containers.find((c) => c.default);
    if (defaultLayout) {
      defaultLayout.default = false;
    }
    let initialLayout;

    const alreadySelectedLayout = this.localStorageService.getItem('LPX_LAYOUT');
    if (alreadySelectedLayout) {
      initialLayout = this.containers.find(
        (c) => c.layoutClass === alreadySelectedLayout
      );
    } else {
      initialLayout = defaultLayout;
    }

    if (initialLayout) {
      initialLayout.default = true;
      this.changeLayout(initialLayout);
    }
  }

  changeLayout(container: LpxContainer): void {
    this.layoutService.removeClasses(this.containerWidthTypes);
    this.layoutService.addClass(container.layoutClass);
    this.localStorageService.setItem('LPX_LAYOUT', container.layoutClass);
    this.store.patch({ selectedLayout: container });
  }
}
