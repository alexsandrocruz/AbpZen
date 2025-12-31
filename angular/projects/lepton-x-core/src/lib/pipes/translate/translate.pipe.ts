import { Pipe, PipeTransform } from '@angular/core';
import { LpxThemeTranslateService } from '../../services';

@Pipe({
  name: 'lpxTranslate',
})
export class TranslatePipe implements PipeTransform {
  constructor(private lpxThemeTranslateService: LpxThemeTranslateService) {}

  transform(value: string, ...args: any[]) {
    return this.lpxThemeTranslateService.translate$(value, args);
  }
}
