import {
  ChangeDetectorRef,
  Directive,
  Input,
  OnDestroy,
  OnInit,
  Optional,
  SkipSelf,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { map } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { ResponsiveService } from './responsive.service';

@Directive({
  selector: '[lpxResponsive]',
})
export class ResponsiveDirective implements OnInit, OnDestroy {
  hasRendered = false;
  sub = new Subscription();

  @Input('lpxResponsive') query!: string;

  render = (shouldRender: boolean) => {
    if (shouldRender && !this.hasRendered) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasRendered = true;
    } else if (!shouldRender && this.hasRendered) {
      this.viewContainer.clear();
      this.hasRendered = false;
    }
    this.parentCdr.detectChanges();
  };

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private service: ResponsiveService,
    @Optional() @SkipSelf() private parentCdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.sub.add(
      this.service.currentSize$
        .pipe(map(_ => this.service.shouldRenderWithCurrentSize(this.query)))
        .subscribe(this.render),
    );
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
