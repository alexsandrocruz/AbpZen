import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthLayoutComponent } from './auth-layout.component';
import { AUTH_LAYOUT_IMG } from './tokens/auth-layout-img.token';

export interface LpxAuthLayoutOptions {
  authLayoutImg?: string;
}

@NgModule({
  declarations: [AuthLayoutComponent],
  imports: [CommonModule],
  exports: [AuthLayoutComponent],
})
export class LpxAuthLayoutModule {
  static forRoot(
    options?: LpxAuthLayoutOptions
  ): ModuleWithProviders<LpxAuthLayoutModule> {
    return {
      ngModule: LpxAuthLayoutModule,
      providers: [
        {
          provide: AUTH_LAYOUT_IMG,
          useValue: options?.authLayoutImg || '',
        },
      ],
    };
  }
}
