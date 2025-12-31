import { ReplaceableComponentsService } from "@abp/ng.core";
import { APP_INITIALIZER, Injector } from "@angular/core";
import { eThemeLeptonXComponents } from "../enums";
import { AccountLayoutComponent } from "@volosoft/abp.ng.theme.lepton-x/account";

export const ACCOUNT_LAYOUT_PROVIDER = {
    provide: APP_INITIALIZER,
    useFactory: initAccountLayout,
    deps: [Injector],
    multi: true,
  }
  
  export function initAccountLayout(injector: Injector) {
    function init() {
      const replaceableComponents = injector.get(ReplaceableComponentsService);
      replaceableComponents.add({
        key: eThemeLeptonXComponents.AccountLayout,
        component: AccountLayoutComponent,
      });
    }
    return init;
  }
  