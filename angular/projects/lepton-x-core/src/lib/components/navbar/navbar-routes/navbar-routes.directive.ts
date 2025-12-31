import { Directive } from '@angular/core';

@Directive({
  selector: '[lpx-navbar-routes],[lpxNavbarRoutes]',
  exportAs: 'lpxNavbarRoutes',
})
export class NavbarRoutesDirective {}
