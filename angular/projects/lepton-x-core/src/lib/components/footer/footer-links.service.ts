import { Injectable } from '@angular/core';
import { DataStore } from '../../utils/data-store';

export interface FooterLink {
  link: string;
  text: string;
}

export interface FooterNav {
  descUrl?: string;
  desc: string;
  footerLinks?: FooterLink[];
}

@Injectable({
  providedIn: 'root',
})
export class FooterLinksService {
  private store = new DataStore({} as FooterNav);
  footerInfo$ = this.store.sliceState((state) => state);

  setFooterInfo(links: FooterNav) {
    this.store.set(links);
  }
}
