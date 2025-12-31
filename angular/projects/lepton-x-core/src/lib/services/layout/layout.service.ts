import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataStore } from '../../utils/data-store';

export interface LayoutSettings {
  containerClass: string;
}

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  readonly store = new DataStore({
    containerClass: [''],
  });

  containerClass$: Observable<Array<string>> = this.store.sliceState(
    ({ containerClass }) => containerClass || [],
  );

  setClass(cssClass: string | Array<string>): void {
    const containerClass = Array.isArray(cssClass) ? cssClass : [cssClass];
    this.patchStore(containerClass);
  }

  addClass(cssClass: string): void {
    const { containerClass } = this.store.state;
    this.patchStore([...containerClass, cssClass]);
  }

  removeClass(cssClass: string): void {
    const { containerClass } = this.store.state;
    const index = containerClass.findIndex((item) => item === cssClass);
    if (index === -1) return;
    const update = [
      ...containerClass.slice(0, index),
      ...containerClass.slice(index + 1),
    ];
    this.patchStore(update);
  }

  removeClasses(classlist: string[]) {
    const { containerClass } = this.store.state;
    const filteredClasslist = containerClass.filter(
      (clss) => !classlist.includes(clss),
    );
    this.patchStore(filteredClasslist);
  }

  toggleClass(cssClass: string): void {
    const { containerClass } = this.store.state;
    const index = containerClass.findIndex((item) => item === cssClass);
    if (index === -1) {
      this.addClass(cssClass);
    } else {
      this.removeClass(cssClass);
    }
  }

  patchStore(containerClass: string[]) {
    this.store.patch({
      containerClass,
    });
  }
}
