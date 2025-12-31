import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { ContextMenuHeaderComponent } from './components/context-menu-header.component';

@Component({
  selector: 'lpx-context-menu',
  templateUrl: './context-menu.component.html',
  encapsulation: ViewEncapsulation.None,
  exportAs: 'lpx-context-menu',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContextMenuComponent {
  protected readonly cdRef = inject(ChangeDetectorRef)
  @ContentChild(ContextMenuHeaderComponent) header?: ContextMenuHeaderComponent;
  private _visible = false;

  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    this._visible = value;
    this.cdRef.detectChanges();
  }

  open(): void {
    this.visible = true;
  }
  close(): void {
    this.visible = false;
  }
  toggle(status?: boolean): void {
    this.visible = status !== undefined ? status : !this._visible;
  }
}
