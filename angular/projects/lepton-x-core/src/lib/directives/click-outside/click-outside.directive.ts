import { Directive, Output, EventEmitter, HostListener, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[lpxClickOutside]',
})
export class ClickOutsideDirective {
  @Output() lpxClickOutside = new EventEmitter();
  @Input()
  exceptedRefs: Array<ElementRef> = [];

  constructor(private elementRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (
      !(
        this.elementRef.nativeElement.contains(event.target) ||
        this.exceptedRefs.some(ref => ref.nativeElement.contains(event.target))
      )
    ) {
      this.lpxClickOutside.emit();
    }
  }
}
