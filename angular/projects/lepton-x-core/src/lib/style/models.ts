export interface LpxStyle {
  bundleName: string;
}

export type LpxStyles = Array<LpxStyle>;

export type LpxStyleFactory<T = LpxStyles> = (styles: T) => T;

export interface HasStyleFactory<T = LpxStyles> {
  styleFactory?: LpxStyleFactory<T>;
}
