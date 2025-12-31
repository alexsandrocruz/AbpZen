import { from, Observable, of } from 'rxjs';
import { ArrayTypeDeep, RecursiveArray, VariantSource } from '../models/common';

export function sortItems<T extends { order?: number }>(a: T, b: T): number {
  if (!a.order) {
    return 1;
  }
  if (!b.order) {
    return -1;
  }

  return a.order - b.order;
}

export function flatArrayDeepToObject<T extends { [key: string]: any }>(
  arr: RecursiveArray<ArrayTypeDeep<T>>
): ArrayTypeDeep<T> {
  return arr.reduce(
    (acc, curr) => ({
      ...acc,
      ...(Array.isArray(curr) ? flatArrayDeepToObject(curr) : curr),
    }),
    {}
  ) as ArrayTypeDeep<T>;
}

export function getStream$<T>(source: VariantSource<T>): Observable<T> {
  return source instanceof Observable
    ? source
    : source instanceof Promise
    ? from(source)
    : of(source);
}

export function isNullOrUndefined<T>(obj: T) {
  return obj === null || obj === undefined;
}

export function isArray<T>(obj: T): boolean {
  return Array.isArray(obj);
}
