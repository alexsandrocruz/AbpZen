import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BreadcrumbPanelDirective } from './breadcrumb-panel.directive';
import { NavbarPanelDirective } from './navbar-panel.directive';
import { FooterPanelDirective } from './footer-panel.directive';
import { MobileNavbarPanelDirective } from './mobile-navbar-panel.directive';
import { ToolbarPanelDirective } from './toolbar-panel.directive';
import { TopNavbarPanelDirective } from './top-navbar-panel.directive';
import { SettingsPanelDirective } from './settings-panel.directive';
import { NavitemPanelDirective } from './navitem-panel.directive';
import { LogoPanelDirective } from './logo-panel.directive';
import { CurrentUserPanelDirective } from './current-user-panel.directive';
import { CurrentUserImagePanelDirective } from './current-user-image-panel.directive';
import { LanguagePanelDirective } from './language-panel.directive';
import { ContentPanelDirective } from './content-panel.directive';
import { MobileNavbarSettingsPanelDirective } from './mobile-navbar-settings-panel.directive';
import { MobileNavbarProfilePanelDirective } from './mobile-navbar-profile-panel.directive';

const declarationsAndExports = [
  BreadcrumbPanelDirective,
  ContentPanelDirective,
  CurrentUserImagePanelDirective,
  CurrentUserPanelDirective,
  FooterPanelDirective,
  LanguagePanelDirective,
  LogoPanelDirective,
  MobileNavbarPanelDirective,
  MobileNavbarSettingsPanelDirective,
  MobileNavbarProfilePanelDirective,
  NavbarPanelDirective,
  NavitemPanelDirective,
  SettingsPanelDirective,
  TopNavbarPanelDirective,
  ToolbarPanelDirective,
];

@NgModule({
  declarations: [...declarationsAndExports],
  imports: [CommonModule],
  exports: [...declarationsAndExports],
})
export class PanelsModule {}
