import { ModuleWithProviders, NgModule, Type } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LpxBrandLogoModule } from '../brand-logo/brand-logo.module';
import { LpxIconModule } from '../icon/icon.module';
import { NavbarComponent } from './navbar.component';
import { SubNavbarComponent } from './sub-navbar/sub-navbar.component';
import { NavbarRoutesComponent } from './navbar-routes/navbar-routes.component';
import { NavbarRoutesDirective } from './navbar-routes/navbar-routes.directive';
import { LpxNavbarItem } from './models';
import {
  CONTENT_AFTER_ROUTES,
  CONTENT_BEFORE_ROUTES,
  LPX_MENU_ITEMS,
} from './navbar.token';
import { ToObservableModule } from '../../pipes/to-observable/to-observable.module';
import { LpxTranslateModule } from '../../pipes';
import { LpxVisibleDirective } from '../../directives/visible.directive';

const exportedDeclarations = [
  NavbarComponent,
  SubNavbarComponent,
  NavbarRoutesComponent,
  NavbarRoutesDirective,
];

export interface LpxNavbarSettings {
  menuItems?: LpxNavbarItem[];
  contentAfterRoutes?: Type<any>[];
  contentBeforeRoutes?: Type<any>[];
}

@NgModule({
  declarations: [...exportedDeclarations],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    LpxBrandLogoModule,
    LpxIconModule,
    ToObservableModule,
    LpxTranslateModule,
    LpxVisibleDirective,
  ],
  exports: [...exportedDeclarations],
})
export class LpxNavbarModule {
  static forRoot(
    options: LpxNavbarSettings = {},
  ): ModuleWithProviders<LpxNavbarModule> {
    return {
      ngModule: LpxNavbarModule,
      providers: [
        {
          provide: LPX_MENU_ITEMS,
          useValue: options?.menuItems || [],
        },
        {
          provide: CONTENT_AFTER_ROUTES,
          useValue: options?.contentAfterRoutes || [],
          multi: true,
        },
        {
          provide: CONTENT_BEFORE_ROUTES,
          useValue: options?.contentBeforeRoutes || [],
          multi: true,
        },
      ],
    };
  }

  static forChild(
    options: Omit<LpxNavbarSettings, 'menuItems'> = {},
  ): ModuleWithProviders<LpxNavbarModule> {
    return {
      ngModule: LpxNavbarModule,
      providers: [
        {
          provide: CONTENT_AFTER_ROUTES,
          useValue: options?.contentAfterRoutes || [],
          multi: true,
        },
        {
          provide: CONTENT_BEFORE_ROUTES,
          useValue: options?.contentBeforeRoutes || [],
          multi: true,
        },
      ],
    };
  }
}
