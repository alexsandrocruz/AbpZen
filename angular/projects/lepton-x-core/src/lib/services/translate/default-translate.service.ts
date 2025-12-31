import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TranslateService } from './models';

@Injectable()
export class DefaultTranslateService implements TranslateService {
  get$(key: string, defaultValue?: string): Observable<string> {
    return of(defaultValue || key || '');
  }

  get(key: string, defaultValue?: string): string {
    return defaultValue || key || '';
  }
}
