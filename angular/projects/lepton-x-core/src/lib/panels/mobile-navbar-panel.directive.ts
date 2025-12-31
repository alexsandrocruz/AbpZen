import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-mobile-navbar-panel]',
})
export class MobileNavbarPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
