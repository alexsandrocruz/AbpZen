import { Component, Injector, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { AuthService, ConfigStateService, EnvironmentService, ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { IdentityLinkUserService, LinkUserDto } from '@volo/abp.ng.account/public/proxy';
import { LinkLoginHandler } from '../handlers/link-login.handler';

@Component({
  selector: 'abp-my-link-users-modal',
  templateUrl: './my-link-users-modal.component.html',
  providers: [ListService],
})
export class MyLinkUsersModalComponent implements OnInit {
  private _isModalOpen = true;
  get isModalOpen() {
    return this._isModalOpen;
  }
  set isModalOpen(value: boolean) {
    if (value && !this._isModalOpen) this.getAllList();
    this._isModalOpen = value;
  }

  readonly list: ListService;
  protected linkUserService: IdentityLinkUserService;
  protected confirmation: ConfirmationService;
  protected configState: ConfigStateService;
  protected authService: AuthService;
  protected environment: EnvironmentService;
  protected linkLoginHandler: LinkLoginHandler;

  constructor(private injector: Injector) {
    this.list = injector.get(ListService);
    this.list.maxResultCount = 5;

    this.linkUserService = injector.get(IdentityLinkUserService);
    this.confirmation = injector.get(ConfirmationService);
    this.configState = injector.get(ConfigStateService);
    this.authService = injector.get(AuthService);
    this.environment = injector.get(EnvironmentService);
    this.linkLoginHandler = injector.get(LinkLoginHandler);
  }

  data: LinkUserDto[] = [];
  filteredData: LinkUserDto[] = [];
  filterText = '';
  resultCount = 0;

  get currentUser() {
    return this.configState.getAll().currentUser;
  }

  get currentTenant() {
    return this.configState.getAll().currentTenant;
  }

  get pageStartIndex() {
    return this.list.page * this.list.maxResultCount;
  }

  get pageEndIndex() {
    return this.pageStartIndex + this.list.maxResultCount;
  }

  ngOnInit() {
    this.list.maxResultCount = 5;
    this.getAllList();
  }

  private navigateToLogin() {
    this.linkUserService.generateLinkToken().subscribe(token => {
      if (this.authService.isInternalAuth) {
        const { id: linkUserId, userName: linkUserName } = this.currentUser;
        const { id: linkTenantId, name: linkTenantName } = this.currentTenant;

        this.authService.logout().subscribe(() => {
          this.authService.navigateToLogin({
            linkToken: token,
            linkUserId,
            linkUserName,
            ...(linkTenantId && {
              linkTenantId,
              linkTenantName,
            }),
          });
          this.isModalOpen = false;
        });

        return;
      }

      const { redirectUri } = this.environment.getEnvironment().oAuthConfig;
      const issuer = this.environment.getIssuer();
      const returnUrl = `${redirectUri}?handler=linkLogin&linkUserId=${this.currentUser.id}${
        this.currentTenant.id ? '&linkTenantId=' + this.currentTenant.id : ''
      }`;

      const fullUrl = `${issuer}Account/Login?LinkUserId=${this.currentUser.id}${
        this.currentTenant.id ? '&LinkTenantId=' + this.currentTenant.id : ''
      }&LinkToken=${encodeURIComponent(token)}&ReturnUrl=${encodeURIComponent(returnUrl)}`;

      window.open(fullUrl, '_self');
    });
  }

  filterByText() {
    this.filteredData = this.getPageData(this.pageStartIndex, this.pageEndIndex);
  }

  getFilteredData() {
    const list = this.data.filter(linkUser =>
      linkUser.targetUserName.toLowerCase().includes(this.filterText.toLowerCase()),
    );
    this.resultCount = list.length;
    return list;
  }

  getPageData(start: number, end: number) {
    return this.getFilteredData().slice(start, end);
  }

  getAllList() {
    this.linkUserService.getAllList().subscribe(res => {
      this.data = res.items;
      this.filteredData = this.getPageData(this.pageStartIndex, this.pageEndIndex);
    });
    this.list
      .hookToQuery(query =>
        of({ items: this.getPageData(query.skipCount, query.skipCount + query.maxResultCount) }),
      )
      .subscribe(res => (this.filteredData = res.items));
  }

  createNewLinkUser() {
    this.confirmation
      .warn('AbpAccount::NewLinkAccountWarning', 'AbpUi::AreYouSure')
      .subscribe(res => {
        if (res === Confirmation.Status.confirm) {
          this.navigateToLogin();
        }
      });
  }

  delete(record: LinkUserDto) {
    this.confirmation
      .warn('AbpAccount::DeleteLinkAccountConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [record.targetUserName],
      })
      .subscribe(res => {
        if (res === Confirmation.Status.confirm) {
          this.linkUserService
            .unlink({ userId: record.targetUserId, tenantId: record.targetTenantId })
            .subscribe(() => this.getAllList());
        }
      });
  }

  LoginAsThisAccount(record: LinkUserDto) {
    this.linkLoginHandler
      .linkLogin({
        linkUserId: record.targetUserId,
        linkTenantId: record.targetTenantId,
      })
      .subscribe(() => (this.isModalOpen = false));
  }
}
