import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-mobile-navbar-profile-panel]',
})
export class MobileNavbarProfilePanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
