import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-mobile-navbar-settings-panel]',
})
export class MobileNavbarSettingsPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
