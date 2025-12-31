import { LpxStyleFactory, LpxStyles } from './models';

export function createStyleFactory<T = LpxStyles>(
  handler?: LpxStyleFactory<T>
): LpxStyleFactory<T> {
  return handler || ((defaultValue: T) => defaultValue);
}
