import {Provider} from "@angular/core";
import {LPX_AUTH_SERVICE_TOKEN} from "./tokens";
import {DefaultAuthService} from "./default-auth.service";

export const LPX_AUTH_SERVICE_PROVIDER: Provider = {
  provide: LPX_AUTH_SERVICE_TOKEN,
  useClass: DefaultAuthService,
};
