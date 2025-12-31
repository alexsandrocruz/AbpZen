import { APP_INITIALIZER, Provider } from '@angular/core';
import {
  AbpNavbarService,
  AbpToolbarService,
} from '@volo/abp.ng.lepton-x.core';
import { AbpSettingsService } from '../services';

export const INIT_SERVICE_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initServices,
  deps: [AbpNavbarService, AbpToolbarService, AbpSettingsService],
  multi: true,
};

export function initServices(
  navbar: AbpNavbarService,
  abpToolbar: AbpToolbarService,
  setting: AbpSettingsService,
) {
  function init() {
    abpToolbar.listenNavItems();
    setting.setUpListeners();
    navbar.initRoutes();
  }

  return init;
}
