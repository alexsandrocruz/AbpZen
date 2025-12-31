import { Inject, Injectable } from '@angular/core';
import { LPX_STYLE_FINAL } from './tokens';
import { LpxStyle, LpxStyles } from './models';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StyleService {
  lastInjectedStyle: HTMLLinkElement | null = null;

  initialized$ = new BehaviorSubject<boolean>(false);

  constructor(
    @Inject(LPX_STYLE_FINAL) private initialStyles: LpxStyles,
    @Inject(DOCUMENT) private document: Document
  ) {}

  async initStyles(direction: 'rtl' | 'ltr') {
    for (const style of this.initialStyles) {
      await this.loadStyle(style, direction);
    }
    this.initialized$.next(true);
  }

  async loadStyle(
    style: LpxStyle,
    direction: 'ltr' | 'rtl'
  ): Promise<HTMLStyleElement | void> {
    return new Promise((resolve, reject) => {
      const linkElem = this.createLinkElem(style, direction, resolve);
      //TODO: find a better way for understand style laaded by angular json
      const appStyles = document.querySelector(
        'link[rel="stylesheet"][href*="styles"]'
      );
      if (appStyles) {
        if (this.lastInjectedStyle && this.lastInjectedStyle.isConnected) {
          this.lastInjectedStyle.insertAdjacentElement('afterend', linkElem);
        } else {
          appStyles.insertAdjacentElement('beforebegin', linkElem);
        }
      } else {
        this.document.head.appendChild(linkElem);
      }
      this.lastInjectedStyle = linkElem;
      return Promise.resolve(linkElem);
    });
  }

  async replaceStyle<T extends LpxStyle>(
    style: T,
    direction: 'rtl' | 'ltr'
  ): Promise<HTMLStyleElement | void> {
    const loaded = this.document.querySelector(`link#${style.bundleName}`);
    if (loaded) {
      loaded.remove();
    }
    return this.loadStyle(style, direction);
  }

  async reloadInitialStyles(direction: 'rtl' | 'ltr') {
    for (const style of this.initialStyles) {
      await this.replaceStyle(style, direction);
    }
  }

  createLinkElem(
    style: LpxStyle,
    direction: 'rtl' | 'ltr',
    resolve: (param: any) => void
  ): HTMLLinkElement {
    const linkElem = document.createElement('link');
    linkElem.rel = 'stylesheet';
    linkElem.id = style.bundleName;
    linkElem.href = `${style.bundleName}${
      direction === 'rtl' ? '.rtl' : ''
    }.css`;
    linkElem.onload = () => {
      resolve(linkElem);
    };
    return linkElem;
  }
}
