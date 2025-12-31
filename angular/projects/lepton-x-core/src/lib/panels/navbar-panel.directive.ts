import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-navbar-panel]',
})
export class NavbarPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
