import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  Input,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { LanguageService, LpxNavbarItem } from '@volo/ngx-lepton-x.core';
import { computePosition, Placement } from '@floating-ui/dom';

@Component({
  selector: 'lpx-top-menu-nav-item',
  templateUrl: './top-menu-nav-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LpxTopMenuNavItemComponent {
  constructor(
    private languageService: LanguageService,
    private renderer2: Renderer2
  ) {}

  @Input()
  public isInnerMenu = false;
  @Input()
  menuItem!: LpxNavbarItem;

  get isRTL() {
    return !!this.languageService.selectedLanguage?.isRTL;
  }

  @HostListener('mouseover')
  onHover() {
    this.calculatePosition();
  }

  @ViewChild('floating')
  floating: ElementRef | undefined;

  @ViewChild('reference')
  reference!: ElementRef;

  private calculatePosition() {
    const placement = this.getPosition();
    const reference = this.reference.nativeElement;
    if (!this.floating) {
      return;
    }
    const floating = this.floating.nativeElement;
    computePosition(reference, floating, {
      placement,
    }).then(({ x, y }) => {
      this.renderer2.setStyle(floating, 'top', `${y}px`);
      this.renderer2.setStyle(floating, 'left', `${x}px`);
    });
  }

  getPosition(): Placement {
    const direction = this.isRTL ? 'left' : 'right';
    return this.isInnerMenu ? `${direction}-start` : 'bottom-start';
  }
}
