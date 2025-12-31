import {
  AfterViewChecked,
  Component,
  ContentChild,
  InjectionToken,
  Injector,
  PLATFORM_ID,
  TemplateRef,
  Type,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { map } from 'rxjs';
import { LayoutService } from '../../services/layout';
import { NavbarService } from './navbar.service';
import { NavbarRoutesDirective } from './navbar-routes/navbar-routes.directive';
import { LogoPanelDirective } from '../../panels/logo-panel.directive';
import { CONTENT_AFTER_ROUTES, CONTENT_BEFORE_ROUTES } from './navbar.token';

@Component({
  selector: 'lpx-navbar',
  templateUrl: './navbar.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class NavbarComponent implements AfterViewChecked {
  protected readonly layoutService = inject(LayoutService);
  protected readonly platformId = inject<Object>(PLATFORM_ID);
  public readonly service = inject(NavbarService);
  public readonly injector = inject(Injector);

  @ContentChild(NavbarRoutesDirective, { read: TemplateRef })
  routesTemplate?: TemplateRef<NavbarRoutesDirective>;

  @ContentChild(LogoPanelDirective) logoPanel?: LogoPanelDirective;

  protected didResized = false;
  protected initialHover = false;

  contentBefore: Type<any>[];
  contentAfter: Type<any>[];
  showFilterMenu$ = this.service.navbarItems$.pipe(
    map((items) => !!items.length),
  );

  constructor() {
    this.contentBefore = this.flatContents(CONTENT_BEFORE_ROUTES);
    this.contentAfter = this.flatContents(CONTENT_AFTER_ROUTES);
  }

  toggleSidebarHover() {
    this.didResized = true;
    this.layoutService.toggleClass('hover-trigger');
    this.initialHover = !this.initialHover;

    if (this.initialHover) {
      this.layoutService.addClass('initial-hover');
    } else {
      this.layoutService.removeClass('initial-hover');
    }
  }

  handleInitialHover() {
    if (this.initialHover) {
      this.layoutService.removeClass('initial-hover');
    }
  }

  ngAfterViewChecked() {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    if (this.didResized) {
      this.didResized = false;
      window.dispatchEvent(new Event('resize'));
    }
  }

  private flatContents(token: InjectionToken<Type<any>[][]>) {
    const contents = this.injector.get(token, []);
    return contents.reduce((acc, val) => acc.concat(val), []);
  }
}
