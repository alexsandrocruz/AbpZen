import {
  APP_INITIALIZER,
  Injector,
  ModuleWithProviders,
  NgModule,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { BreadcrumbComponent } from './breadcrumb.component';
import { LpxIconModule } from '../icon/icon.module';
import { RouterModule } from '@angular/router';
import { LpxClickOutsideModule } from '../../directives/click-outside/click-outside.module';
import { BreadcrumbRouteListenerService } from './breadcrumb-route-listener.service';
import { ToObservableModule } from '../../pipes/to-observable/to-observable.module';

export const exportedDeclarations = [BreadcrumbComponent];
@NgModule({
  declarations: [...exportedDeclarations],
  imports: [
    CommonModule,
    LpxIconModule,
    ToObservableModule,
    RouterModule,
    LpxClickOutsideModule,
  ],
  exports: [...exportedDeclarations],
})
export class LpxBreadcrumbModule {
  static forRoot(): ModuleWithProviders<LpxBreadcrumbModule> {
    return {
      ngModule: LpxBreadcrumbModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          useFactory: breadCrumbInit,
          multi: true,
          deps: [Injector],
        },
      ],
    };
  }
}

export function breadCrumbInit(injector: Injector) {
  const subs = () => {
    const service = injector.get(BreadcrumbRouteListenerService);
    service.subscribeRoute();
  };
  return subs;
}
