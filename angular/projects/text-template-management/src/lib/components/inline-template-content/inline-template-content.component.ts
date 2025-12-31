import { Component, OnInit, Injector, inject } from '@angular/core';
import { AbstractTemplateContentComponent } from '../abstract-template-content/abstract-template-content.component';
import { Router } from '@angular/router';

@Component({
  selector: 'abp-inline-template-content',
  templateUrl: 'inline-template-content.component.html',
})
export class InlineTemplateContentComponent
  extends AbstractTemplateContentComponent
  implements OnInit
{
  private readonly router = inject(Router);

  customizePerCulture() {
    this.router.navigateByUrl(
      `/text-template-management/text-templates/contents/${this.templateContent.name}`,
    );
  }

  save() {
    super.save(() => {
      this.router.navigateByUrl('/text-template-management/text-templates');
    });
  }
}
