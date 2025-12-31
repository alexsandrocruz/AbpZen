import { Inject, Injectable } from '@angular/core';
import { LocalizationService } from '@abp/ng.core';
import {
  flatArrayDeepToObject,
  TranslateService,
} from '@volo/ngx-lepton-x.core';
import { Observable, of } from 'rxjs';
import { LPX_TRANSLATE_KEY_MAP_TOKEN } from '../tokens/translate-key-map';

@Injectable()
export class AbpTranslateService implements TranslateService {
  constructor(
    private localization: LocalizationService,
    @Inject(LPX_TRANSLATE_KEY_MAP_TOKEN)
    private translateKeys: Array<any>
  ) {}

  get(key: string, defaultValue: string | undefined): string {
    const keyToTranslate = this.getKey(key);
    if (keyToTranslate) {
      return this.localization.instant({
        key: keyToTranslate,
        defaultValue,
      });
    }

    return defaultValue || key;
  }

  get$(key: string, defaultValue: string | undefined): Observable<string> {
    const keyToTranslate = this.getKey(key);
    if (keyToTranslate) {
      return this.localization.get({
        key: keyToTranslate,
        defaultValue,
      });
    }

    return of(defaultValue || key);
  }

  private getKey(key: string): string | undefined {
    const keys = flatArrayDeepToObject(this.translateKeys);
    return keys[key] ? keys[key] : key.includes('::') ? key : undefined;
  }
}
