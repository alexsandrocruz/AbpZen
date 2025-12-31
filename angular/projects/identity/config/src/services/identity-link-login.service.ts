import { Injectable } from '@angular/core';
import { AuthService } from '@abp/ng.core';
import { Public } from '@volo/abp.ng.account/public/proxy';
import { from } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class IdentityLinkLoginService {
  constructor(private authService: AuthService) {}

  linkLogin(input: Public.Web.Areas.Account.Controllers.Models.LinkUserLoginInfo) {
    const grantType = 'LinkLogin';
    const params = {
      LinkUserId: input.linkUserId,
    };

    if (input.linkTenantId) {
      params['LinkTenantId'] = input.linkTenantId;
    }

    return  from(this.authService.loginUsingGrant(grantType, params));
  }
}
