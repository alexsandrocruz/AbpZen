import { ElementRef, Injectable, OnDestroy } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
} from '@angular/router';
import { Subscription, filter } from 'rxjs';

import { LpxPerfectScrollbarOptions } from '../models';
import { LpxPerfectScrollbar } from '../abstracts';

import PerfectScrollbar from 'perfect-scrollbar';

@Injectable()
export class LpxPerfectScrollbarService
  extends LpxPerfectScrollbar
  implements OnDestroy
{
  private subscription: Subscription | undefined;

  setElement(value: ElementRef<any>): void {
    if (value) {
      this.elementRef = value;
    }
  }

  setOptions(value: LpxPerfectScrollbarOptions): void {
    if (value) {
      this.options = value;
    }
  }

  createScrollbar(): void {
    this.perfectScrollbar = new PerfectScrollbar(
      this.elementRef.nativeElement,
      this.options
    );
  }

  onResize(): void {
    this.perfectScrollbar.update();
  }

  afterViewInit(): void {
    this.createScrollbar();

    this.subscription?.unsubscribe();
    this.subscription = this.router.events
      .pipe(
        filter(
          (event) =>
            event instanceof NavigationEnd ||
            event instanceof NavigationError ||
            event instanceof NavigationCancel
        )
      )
      .subscribe(() => {
        const { element } = this.perfectScrollbar;
        const { topAfterNavigate, leftAfterNavigate } = this.options || {};
        element.scrollTop = topAfterNavigate || 0;
        element.scrollLeft = leftAfterNavigate || 0;
      });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
