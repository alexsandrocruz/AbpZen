import { Observable } from 'rxjs';
import PerfectScrollbar from 'perfect-scrollbar';

export type VariantSource<T> = Observable<T> | Promise<T> | T;

export interface RecursiveArray<T> extends Array<T | RecursiveArray<T>> {
  [index: number]: RecursiveArray<T> | T;
  length: number;
}

export type ArrayTypeDeep<T> = T extends Array<infer U> ? ArrayTypeDeep<U> : T;

export interface LpxPerfectScrollbarOptions extends PerfectScrollbar.Options {
  leftAfterNavigate?: number;
  topAfterNavigate?: number;
}
