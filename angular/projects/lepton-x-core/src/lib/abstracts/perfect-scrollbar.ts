import { ElementRef, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import PerfectScrollbar from 'perfect-scrollbar';
import { LpxPerfectScrollbarOptions } from '../models';

export abstract class LpxPerfectScrollbar {
  protected readonly router = inject(Router);

  protected elementRef!: ElementRef<any>;
  protected perfectScrollbar!: PerfectScrollbar;
  protected options: LpxPerfectScrollbarOptions | undefined;

  abstract setElement(value: ElementRef<any>): void;
  abstract setOptions(value: LpxPerfectScrollbarOptions): void;
  abstract createScrollbar(): void;
  abstract onResize(): void;
  abstract afterViewInit(): void;
}
