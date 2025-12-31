import {
  ChangeDetectionStrategy,
  Component,
  Input,
  TemplateRef,
} from '@angular/core';
import { TopMenuSubNavbarService } from '../../services/top-menu-sub-navbar.service';
import { LpxNavbarItem } from '@volo/ngx-lepton-x.core';
import { Observable } from "rxjs";

@Component({
  selector: 'lpx-header',
  templateUrl: './header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  menuItems$:Observable<LpxNavbarItem[]> = this.topMenuSubNavbarService.items$;

  @Input()
  navbarTemplateRef: TemplateRef<unknown> | undefined;
  @Input()
  logoTemplateRef: TemplateRef<unknown> | undefined;
  @Input()
  toolbarTemplateRef: TemplateRef<unknown> | undefined;
  @Input()
  topNavbarTemplateRef: TemplateRef<unknown> | undefined;
  @Input()
  settingsTemplateRef: TemplateRef<unknown> | undefined;
  constructor(private topMenuSubNavbarService: TopMenuSubNavbarService) {}
}
