import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-language-panel]',
})
export class LanguagePanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
