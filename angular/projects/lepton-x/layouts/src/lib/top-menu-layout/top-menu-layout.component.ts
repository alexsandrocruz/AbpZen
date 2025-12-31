import {
  Component,
  ChangeDetectionStrategy,
  ContentChild,
  inject,
  ChangeDetectorRef,
  AfterViewInit,
} from '@angular/core';
import {
  BreadcrumbPanelDirective,
  ContentPanelDirective,
  FooterPanelDirective,
  LogoPanelDirective,
  MobileNavbarPanelDirective,
  MobileNavbarProfilePanelDirective,
  MobileNavbarSettingsPanelDirective,
  NavbarPanelDirective,
  SettingsPanelDirective,
  ToolbarPanelDirective,
  TopNavbarPanelDirective,
} from '@volo/ngx-lepton-x.core';

@Component({
  selector: 'lpx-layout',
  templateUrl: './top-menu-layout.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopMenuLayoutComponent implements AfterViewInit {
  private readonly cdr = inject(ChangeDetectorRef);

  @ContentChild(NavbarPanelDirective) navbarPanel?: NavbarPanelDirective;
  @ContentChild(BreadcrumbPanelDirective) breadcrumb?: BreadcrumbPanelDirective;
  @ContentChild(FooterPanelDirective) footer?: FooterPanelDirective;
  @ContentChild(MobileNavbarPanelDirective)
  mobileNavbar?: MobileNavbarPanelDirective;
  @ContentChild(ToolbarPanelDirective) toolbar?: ToolbarPanelDirective;
  @ContentChild(TopNavbarPanelDirective) topNavbar?: TopNavbarPanelDirective;
  @ContentChild(SettingsPanelDirective) settingsPanel?: SettingsPanelDirective;
  @ContentChild(ContentPanelDirective) contentPanel?: ContentPanelDirective;
  @ContentChild(LogoPanelDirective) logo?: LogoPanelDirective;
  @ContentChild(MobileNavbarSettingsPanelDirective)
  mobileNavbarSettingsPanel?: MobileNavbarSettingsPanelDirective;
  @ContentChild(MobileNavbarProfilePanelDirective)
  mobileNavbarProfilePanel?: MobileNavbarProfilePanelDirective;

  ngAfterViewInit(): void {
    this.cdr.markForCheck();
  }
}
