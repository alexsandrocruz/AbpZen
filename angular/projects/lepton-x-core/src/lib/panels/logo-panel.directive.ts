import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-logo-panel]',
})
export class LogoPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
