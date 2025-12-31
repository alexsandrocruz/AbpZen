import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-footer-panel]',
})
export class FooterPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
