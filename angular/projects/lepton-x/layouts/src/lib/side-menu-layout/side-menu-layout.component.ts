import {
  ChangeDetectionStrategy,
  Component,
  ContentChild,
} from '@angular/core';
import { SideMenuLayoutService } from './side-menu-layout.service';
import {
  BreadcrumbPanelDirective,
  ContentPanelDirective,
  CurrentUserImagePanelDirective,
  FooterPanelDirective,
  LogoPanelDirective,
  MobileNavbarPanelDirective,
  MobileNavbarProfilePanelDirective,
  MobileNavbarSettingsPanelDirective,
  NavbarPanelDirective,
  NavitemPanelDirective,
  SettingsPanelDirective,
  ToolbarPanelDirective,
} from '@volo/ngx-lepton-x.core';

@Component({
  selector: 'lpx-layout',
  templateUrl: './side-menu-layout.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideMenuLayoutComponent {
  @ContentChild(NavbarPanelDirective) navbarPanel?: NavbarPanelDirective;
  @ContentChild(BreadcrumbPanelDirective)
  breadcrumbPanel?: BreadcrumbPanelDirective;
  @ContentChild(ToolbarPanelDirective) toolbarPanel?: ToolbarPanelDirective;
  @ContentChild(SettingsPanelDirective) settingsPanel?: SettingsPanelDirective;
  @ContentChild(FooterPanelDirective) footerPanel?: FooterPanelDirective;
  @ContentChild(NavitemPanelDirective) navitemPanel?: NavitemPanelDirective;
  @ContentChild(LogoPanelDirective) logoPanel?: LogoPanelDirective;
  @ContentChild(CurrentUserImagePanelDirective)
  currentUserImagePanel?: CurrentUserImagePanelDirective;
  @ContentChild(MobileNavbarPanelDirective)
  mobileNavbarPanel?: MobileNavbarPanelDirective;
  @ContentChild(MobileNavbarSettingsPanelDirective)
  mobileNavbarSettingsPanel?: MobileNavbarSettingsPanelDirective;
  @ContentChild(MobileNavbarProfilePanelDirective)
  mobileNavbarProfilePanel?: MobileNavbarProfilePanelDirective;
  @ContentChild(ContentPanelDirective)
  contentPanel?: ContentPanelDirective;

  constructor(public layoutService: SideMenuLayoutService) {}
}
