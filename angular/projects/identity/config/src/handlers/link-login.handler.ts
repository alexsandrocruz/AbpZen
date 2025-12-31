import { ToasterService } from '@abp/ng.theme.shared';
import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Public } from '@volo/abp.ng.account/public/proxy';
import { from, of, pipe } from 'rxjs';
import { catchError, filter, switchMap, tap } from 'rxjs/operators';
import { IdentityLinkLoginService } from '../services/identity-link-login.service';

@Injectable({ providedIn: 'root' })
export class LinkLoginHandler {
  private handleLinkLoginError = (err: HttpErrorResponse) => {
    this.toaster.error(err.error?.error);
    return of(null);
  };

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private identityLinkLoginService: IdentityLinkLoginService,
    private toaster: ToasterService,
  ) {
    this.listenToQueryParams();
  }

  private listenToQueryParams() {
    this.route.queryParams
      .pipe(
        filter(params => params.handler === 'linkLogin' && (params.linkUserId || params.token)),
        switchMap(
          (
            params: Public.Web.Areas.Account.Controllers.Models.LinkUserLoginInfo & {
              token?: string;
            },
          ) => {
            if (params.token) {
              Object.entries(JSON.parse(params.token)).forEach(([key, value]: [string, string]) => {
                localStorage.setItem(key, value);
              });
            }

            return params.linkUserId
              ? this.linkLogin(params)
              : of(null).pipe(this.pipeToNavigate());
          },
        ),
      )
      .subscribe();
  }

  linkLogin(input: Public.Web.Areas.Account.Controllers.Models.LinkUserLoginInfo) {
    return this.identityLinkLoginService.linkLogin(input).pipe(
      catchError(this.handleLinkLoginError),
      switchMap(res => {
        if (res.tenant_domain) {
          const now = new Date().valueOf();
          const token = {
            access_token: res.access_token,
            refresh_token: res.refresh_token,
            access_token_stored_at: now,
            expires_at: now + res.expires_in * 1000,
          };
          location.href = `${res.tenant_domain}?handler=linkLogin&token=${JSON.stringify(token)}`;
          return of(null);
        }

        localStorage.setItem('access_token', res.access_token);
        if (res.refresh_token) localStorage.setItem('refresh_token', res.refresh_token);
        return of(null).pipe(this.pipeToNavigate());
      }),
    );
  }

  private pipeToNavigate() {
    return pipe(
      switchMap(() =>
        from(
          this.router.navigate(['.'], {
            relativeTo: this.route,
            queryParams: { handler: null, linkUserId: null, linkTenantId: null, token: null },
            queryParamsHandling: 'merge',
          }),
        ),
      ),
      tap(() => location.reload()),
    );
  }
}
