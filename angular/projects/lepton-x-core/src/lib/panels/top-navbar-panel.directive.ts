import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-top-navbar-panel]',
})
export class TopNavbarPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
