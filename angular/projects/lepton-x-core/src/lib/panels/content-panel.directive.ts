import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-content]',
})
export class ContentPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
