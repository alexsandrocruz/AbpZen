import { of } from 'rxjs';
import { GdprRequestDto } from '@volo/abp.ng.gdpr/proxy';

export const getStatus = (requestDto: GdprRequestDto) => {
  return of(requestDto);
};
