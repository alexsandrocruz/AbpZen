import {
  AfterViewInit,
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { ReplaceableComponentsService } from '@abp/ng.core';

@Directive({
  selector: '[abpIfReplaceableTemplateExists]',
})
export class IfReplaceableTemplateExistsDirective implements AfterViewInit {
  @Input()
  abpIfReplaceableTemplateExists: string;

  constructor(
    private view: ViewContainerRef,
    private template: TemplateRef<any>,
    private replaceableComponentsService: ReplaceableComponentsService
  ) {}

  ngAfterViewInit(): void {
    const replaceableComponentInstance = this.replaceableComponentsService.get(
      this.abpIfReplaceableTemplateExists
    );
    const isReplaceableComponentInstanceNotExits = !replaceableComponentInstance;
    if (isReplaceableComponentInstanceNotExits) {
      return;
    }
    this.view.createEmbeddedView(this.template);
  }
}
