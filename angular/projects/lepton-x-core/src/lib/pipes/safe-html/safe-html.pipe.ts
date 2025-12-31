import { inject, Pipe, PipeTransform, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({ name: 'lpxSafeHtml', standalone: true })
export class SafeHtmlPipe implements PipeTransform {
  private readonly sanitizer = inject(DomSanitizer);
  
  transform(value: string): string {
    if (!value || typeof value !== 'string') return '';
    return this.sanitizer.sanitize(SecurityContext.HTML, value) || '';
  }
}
