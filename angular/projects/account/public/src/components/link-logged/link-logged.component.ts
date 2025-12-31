import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { IdentityLinkUserService } from '@volo/abp.ng.account/public/proxy';
import { getRedirectUrl } from '../../utils/auth-utils';

@Component({
  selector: 'abp-link-logged',
  template: `
    <div class="d-grid">
      <button type="button" class="btn btn-primary mb-3" (click)="navigateToMainPage()">
        &larr;
        <span class="ms-1">{{ 'AbpAccount::StayWithCurrentAccount' | abpLocalization }}</span>
      </button>
      <button (click)="navigateToMainPageForLinkLogin()" class="btn btn-secondary" type="button">
        <span class="me-1">{{
          'AbpAccount::ReturnToPreviousAccount' | abpLocalization: tenantAndUserName
        }}</span>
        &rarr;
      </button>
    </div>
  `,
})
export class LinkLoggedComponent implements OnInit {
  tenantAndUserName = '';

  get linkUser() {
    const { linkUserId, linkTenantId, linkUserName, linkTenantName } =
      this.route.snapshot.queryParams;

    return { linkUserId, linkTenantId, linkUserName, linkTenantName };
  }

  constructor(
    private injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private identityLinkUserService: IdentityLinkUserService,
  ) {}

  ngOnInit() {
    this.init();
  }

  protected init() {
    const { linkUserId, linkTenantId, linkUserName, linkTenantName } = this.linkUser;

    if (!linkUserId) {
      this.navigateToLogin();
      return;
    }

    this.identityLinkUserService
      .isLinked({ tenantId: linkTenantId, userId: linkUserId })
      .subscribe(res => {
        if (!res) {
          this.navigateToLogin();
        }
      });

    this.tenantAndUserName = linkTenantId ? `${linkTenantName}\\${linkUserName}` : linkUserName;
  }

  protected navigateToLogin() {
    this.router.navigateByUrl('/account/login');
  }

  navigateToMainPage(queryParams?: Params) {
    this.router.navigate([getRedirectUrl(this.injector) || '/'], { queryParams });
  }

  navigateToMainPageForLinkLogin() {
    const { linkUserId, linkTenantId } = this.linkUser;
    this.navigateToMainPage({ handler: 'linkLogin', linkUserId, linkTenantId });
  }
}
