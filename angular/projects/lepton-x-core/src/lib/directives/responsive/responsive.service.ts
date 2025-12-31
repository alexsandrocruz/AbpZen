import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, fromEvent, Observable } from 'rxjs';
import { distinctUntilChanged, map, startWith } from 'rxjs/operators';
import {
  RESPONSIVE_BREAKPOINTS,
  ResponsiveBreakpoints,
} from './responsive-breakpoints.token';
import { WINDOW } from '../../tokens/window.token';

export const enum ResponsiveTokens {
  none = 'none',
  all = 'all',
  separator = '-',
}

export interface ResponsiveBreakpoint {
  name: string;
  width: number;
}

@Injectable({
  providedIn: 'root',
})
export class ResponsiveService {
  currentResolution$!: Observable<{ height: number; width: number }>;

  defaultBreakpoint = {
    name: ResponsiveTokens.all,
    width: 0,
  };

  breakpoints: { name: string; width: number }[] = this.buildBreakpoints(
    this.providedBreakpoints,
  );

  protected getCurrentSize = () => ({
    height: this.window.innerHeight,
    width: this.window.innerWidth,
  });

  protected mapSizeToBreakpoint = ({ width } = this.getCurrentSize()) => {
    return this.breakpoints.find(
      (s) => width >= s.width,
    ) as ResponsiveBreakpoint;
  };

  currentSize$ = new BehaviorSubject<ResponsiveBreakpoint>(
    this.mapSizeToBreakpoint(),
  );

  shouldRenderWithCurrentSize = (query: string) => {
    return this.matchQuery(query);
  };

  constructor(
    @Inject(RESPONSIVE_BREAKPOINTS)
    protected providedBreakpoints: ResponsiveBreakpoints,
    @Inject(WINDOW) protected window: any,
  ) {
    this.setupListener();
  }

  protected setupListener() {
    this.currentResolution$ = fromEvent(this.window, 'resize')
      .pipe(map(this.getCurrentSize))
      .pipe(startWith(this.getCurrentSize()));

    this.currentResolution$
      .pipe(map(this.mapSizeToBreakpoint), distinctUntilChanged())
      .subscribe((current) => {
        this.currentSize$.next(current);
      });
  }

  protected buildBreakpoints(breakpoints: ResponsiveBreakpoints) {
    return [
      ...Object.keys(breakpoints)
        .map((key) => ({
          name: key,
          width: breakpoints[key],
        }))
        .sort((a, b) => b.width - a.width),
      this.defaultBreakpoint,
    ];
  }

  protected matchQuery(query: string) {
    const { width } = this.getCurrentSize();
    const tokens = query.split(' ');
    const findInTokens = (size: string) =>
      tokens.find(
        (token) => token.split(ResponsiveTokens.separator)[0] === size,
      );

    const matchedBreakpoint = this.breakpoints.find(
      (breakpoint) =>
        width >= breakpoint.width && findInTokens(breakpoint.name),
    );
    if (matchedBreakpoint) {
      const token = findInTokens(matchedBreakpoint.name);
      const shouldBeBigger = !token?.includes(ResponsiveTokens.none);
      return shouldBeBigger === width >= matchedBreakpoint.width;
    }

    return false;
  }
}
