import { Component } from '@angular/core';
import { PageAlertService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-page-alert-container',
  templateUrl: './page-alert-container.component.html',
})
export class PageAlertContainerComponent {
  replaceableTemplateKey = {
    componentKey: 'Theme.PageAlertContainerComponent',
  };

  constructor(public service: PageAlertService) {}
}
