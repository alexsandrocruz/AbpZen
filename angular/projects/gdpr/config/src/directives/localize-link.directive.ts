import { Directive, Input, OnInit, ViewContainerRef } from '@angular/core';
import { LinkComponent } from '../components/link/link.component';
import { AbpCookieConsentOptions } from '../models/config-module-options';

@Directive({
  selector: '[abpLocalizeLink]',
})
export class LocalizeLinkDirective implements OnInit {
  @Input() link: AbpCookieConsentOptions;

  constructor(private vcRef: ViewContainerRef) {}

  ngOnInit() {
    const createdComponent = this.vcRef.createComponent<LinkComponent>(LinkComponent);
    createdComponent.instance.link = this.link;
  }
}
