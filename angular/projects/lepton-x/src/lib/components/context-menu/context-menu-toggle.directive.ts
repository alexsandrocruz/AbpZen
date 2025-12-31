import { Directive, HostListener, Input } from '@angular/core';
import { ContextMenuComponent } from './context-menu.component';

@Directive({
  selector: '[lpxCtxToggle]',
})
export class ContextMenuToggleDirective {
  @Input('lpxCtxToggle')
  lpxCtx!: ContextMenuComponent;

  @HostListener('click')
  clickOnItem(): void {
    this.lpxCtx.toggle();
  }
}
