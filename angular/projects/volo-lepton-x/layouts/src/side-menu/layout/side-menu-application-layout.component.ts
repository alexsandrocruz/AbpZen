import { Component, ViewEncapsulation } from '@angular/core';
import { eThemeLeptonXComponents } from '@volosoft/abp.ng.theme.lepton-x';

@Component({
  selector: 'abp-application-layout',
  templateUrl: './side-menu-application-layout.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class SideMenuApplicationLayoutComponent {
  toolbarKey = eThemeLeptonXComponents.Toolbar;
  navbarKey = eThemeLeptonXComponents.Navbar;
  routesKey = eThemeLeptonXComponents.Routes;
  navItemsKey = eThemeLeptonXComponents.NavItems;
  breadcrumbKey = eThemeLeptonXComponents.Breadcrumb;
  footerKey = eThemeLeptonXComponents.Footer;
  settingsPanelKey = eThemeLeptonXComponents.Settings;
  mobileNavbarKey = eThemeLeptonXComponents.MobileNavbar;
  mobileNavbarSettingsKey = eThemeLeptonXComponents.MobileNavbarSettings;
  mobileNavbarProfileKey = eThemeLeptonXComponents.MobileNavbarProfile;
  pageAlertContainerKey = eThemeLeptonXComponents.PageAlertContainer;
  logoKey = eThemeLeptonXComponents.Logo;
  currentUserImageKey = eThemeLeptonXComponents.CurrentUserImage;
}
