import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-breadcrumb-panel]',
})
export class BreadcrumbPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
