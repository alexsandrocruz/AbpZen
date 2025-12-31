import { Component, Injector } from '@angular/core';
import { TopbarContentService, TopBarItem } from './topbar-content.service';
import { DomSanitizer } from '@angular/platform-browser';
import { map } from 'rxjs/operators';

@Component({
  selector: 'lpx-topbar-content',
  templateUrl: './topbar-content.component.html',
})
export class TopbarContentComponent {
  items$ = this.topbarContent.items$.pipe(
    map((items) =>
      items.map((item) =>
        item.html && typeof item.html === 'string'
          ? { ...item, html: this.sanitizer.bypassSecurityTrustHtml(item.html) }
          : item
      )
    )
  );

  constructor(
    public topbarContent: TopbarContentService,
    public sanitizer: DomSanitizer,
    public injector: Injector
  ) {}

  actionClick(item: TopBarItem): void {
    if (item.action) {
      item.action();
    }
  }
}
