import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-current-user-image-panel]',
})
export class CurrentUserImagePanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
