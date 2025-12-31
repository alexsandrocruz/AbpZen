import {InjectionToken} from "@angular/core";
import {AuthService} from "./models";

export const LPX_AUTH_SERVICE_TOKEN = new InjectionToken<AuthService>(
  'LPX_AUTH_SERVICE_TOKEN'
);
