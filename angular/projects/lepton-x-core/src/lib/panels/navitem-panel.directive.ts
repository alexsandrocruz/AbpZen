import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-navitem-panel]',
})
export class NavitemPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
