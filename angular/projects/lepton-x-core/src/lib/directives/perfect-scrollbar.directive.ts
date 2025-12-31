import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostListener,
  Input,
  inject,
} from '@angular/core';
import { LpxPerfectScrollbar } from '../abstracts';
import { LPX_PERFECT_SCROLLBAR } from '../tokens';

@Directive({
  selector: '[lpxPerfectScrollbar]',
  standalone: true,
})
export class PerfectScrollbarDirective implements AfterViewInit {
  protected readonly elementRef = inject(ElementRef);
  protected readonly lpxPerfectService = inject<LpxPerfectScrollbar>(
    LPX_PERFECT_SCROLLBAR
  );

  @Input()
  set lpxPerfectScrollbarOptions(value: any) {
    this.lpxPerfectService.setOptions(value);
  }

  @HostListener('window:resize')
  onResize(): void {
    this.lpxPerfectService.onResize();
  }

  ngAfterViewInit(): void {
    this.lpxPerfectService.setElement(this.elementRef);
    this.lpxPerfectService.afterViewInit();
  }
}
