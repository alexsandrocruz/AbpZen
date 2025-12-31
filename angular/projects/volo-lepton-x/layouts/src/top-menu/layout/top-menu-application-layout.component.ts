import { Component } from '@angular/core';
import { eThemeLeptonXComponents } from '@volosoft/abp.ng.theme.lepton-x';

@Component({
  selector: 'abp-top-menu-application-layout',
  templateUrl: './top-menu-application-layout.component.html',
})
export class TopMenuApplicationLayoutComponent {
  navbarKey = eThemeLeptonXComponents.Navbar;
  routesKey = eThemeLeptonXComponents.Routes;
  toolbarKey = eThemeLeptonXComponents.Toolbar;
  topNavbarKey = eThemeLeptonXComponents.TopNavbar;
  breadcrumbKey = eThemeLeptonXComponents.Breadcrumb;
  footerKey = eThemeLeptonXComponents.Footer;
  mobileNavbarKey = eThemeLeptonXComponents.MobileNavbar;
  mobileNavbarSettingsKey = eThemeLeptonXComponents.MobileNavbarSettings;
  mobileNavbarProfileKey = eThemeLeptonXComponents.MobileNavbarProfile;
  pageAlertContainerKey = eThemeLeptonXComponents.PageAlertContainer;
  settingsKey = eThemeLeptonXComponents.Settings;
  logoKey = eThemeLeptonXComponents.Logo;
}
