import { Inject, Injectable, Optional } from '@angular/core';
import { Observable } from 'rxjs';
import { LPX_TRANSLATE_SERVICE_TOKEN, LPX_TRANSLATE_TOKEN } from './tokens';
import { LpxTranslate, TranslateService } from './models';
import { flatArrayDeepToObject } from '../../utils/common';

@Injectable({
  providedIn: 'root',
})
export class LpxThemeTranslateService {
  private _content: LpxTranslate = flatArrayDeepToObject(this.translateValues);
  constructor(
    @Optional()
    @Inject(LPX_TRANSLATE_TOKEN)
    private translateValues: Array<LpxTranslate[]>,
    @Inject(LPX_TRANSLATE_SERVICE_TOKEN)
    private translateService: TranslateService
  ) {}

  // TODO: PROVIDE API : Implement args
  translate$(key: string, ...args: any[]): Observable<string> {
    return this.translateService.get$(key, this._content[key]);
  }
}
