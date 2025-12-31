import { Pipe, PipeTransform } from '@angular/core';
import { Observable, of } from 'rxjs';
import { VariantSource } from '../../models/common';
import { getStream$ } from '../../utils/common';

@Pipe({
  name: 'toObservable',
})
export class ToObservablePipe implements PipeTransform {
  transform(
    value: VariantSource<string> | VariantSource<number> | undefined
  ): Observable<string | number> {
    return value ? getStream$(value) : of('');
  }
}
