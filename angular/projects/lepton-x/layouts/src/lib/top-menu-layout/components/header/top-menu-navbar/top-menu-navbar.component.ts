import { Subscription } from 'rxjs';
import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { NavbarService } from '@volo/ngx-lepton-x.core';

@Component({
  selector: 'lpx-top-menu-navbar',
  templateUrl: './top-menu-navbar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopMenuNavbarComponent implements OnDestroy {
  menuItems$ = this.navbarService.navbarItems$;
  subscription: Subscription;

  constructor(private navbarService: NavbarService) {
    this.subscription = this.navbarService.expandItemByLink$().subscribe();
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
