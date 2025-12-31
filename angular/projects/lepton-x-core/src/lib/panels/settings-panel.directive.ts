import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[lpx-settings-panel]',
})
export class SettingsPanelDirective {
  constructor(public template: TemplateRef<any>) {}
}
