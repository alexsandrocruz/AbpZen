import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-current-user-panel]',
})
export class CurrentUserPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
