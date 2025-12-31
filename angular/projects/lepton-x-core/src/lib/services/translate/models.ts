import { Observable } from 'rxjs';

export interface LpxTranslate {
  [key: string]: string;
}

export interface TranslateService {
  get$: (key: string, defaultValue?: string) => Observable<string>;
  get: (key: string, defaultValue?: string) => string;
}

export type LpxTranslateContentMapFn = (
  values: Array<LpxTranslate[]>
) => LpxTranslate;
