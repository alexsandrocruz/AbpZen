import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-toolbar-panel]',
})
export class ToolbarPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
