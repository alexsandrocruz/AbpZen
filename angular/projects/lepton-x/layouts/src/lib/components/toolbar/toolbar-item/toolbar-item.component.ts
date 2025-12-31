import { Component, Input, TemplateRef, Type } from '@angular/core';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'lpx-toolbar-item',
  templateUrl: './toolbar-item.component.html',
})
export class ToolbarItemComponent {
  @Input()
  component?: Type<any>;

  @Input()
  template?: TemplateRef<any>;

  @Input()
  icon?: string;

  @Input()
  badge?: number | Observable<number>;

  @Input()
  html?: string;

  @Input()
  action?: () => void;

  get badge$(): Observable<number | undefined> {
    if (this.badge instanceof Observable) {
      return this.badge;
    }
    return of(this.badge);
  }

  actionClick(): void {
    if (this.action) {
      this.action();
    }
  }
}
