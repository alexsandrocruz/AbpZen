import { Component, OnInit, inject, signal } from '@angular/core';
import { filter, switchMap, tap } from 'rxjs';
import { NgbNavModule, NgbDropdownModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import {
  AuthService,
  ConfigStateService,
  CoreModule,
  ListService,
  PagedResultDto,
} from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  AccountSessionService,
  GetAccountIdentitySessionListInput,
  Volo,
} from '@volo/abp.ng.account/public/proxy';
import { AccountSessionDetailComponent } from './account-session-detail-modal/account-session-detail.component';

@Component({
  standalone: true,
  selector: 'abp-account-sessions',
  templateUrl: './account-sessions.component.html',
  imports: [
    NgbDropdownModule,
    NgbTooltipModule,
    NgbNavModule,
    CoreModule,
    ThemeSharedModule,
    AccountSessionDetailComponent,
  ],
  providers: [ListService],
})
export class AccountSessionsComponent implements OnInit {
  protected readonly list = inject(ListService<GetAccountIdentitySessionListInput>);
  protected readonly service = inject(AccountSessionService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly authService = inject(AuthService);
  protected readonly configStateService = inject(ConfigStateService);

  data = signal<PagedResultDto<Volo.Abp.Identity.IdentitySessionDto>>({ items: [], totalCount: 0 });
  sessionId = signal('');
  visibleSessionDetailModal = signal(false);

  protected hookToQuery() {
    this.list.hookToQuery(query => this.service.getList({ ...query })).subscribe(this.data.set);
  }

  ngOnInit(): void {
    this.hookToQuery();
  }

  showDetail(row: Volo.Abp.Identity.IdentitySessionDto) {
    this.visibleSessionDetailModal.set(true);
    this.sessionId.set(row.id);
  }

  revokeSession(row: Volo.Abp.Identity.IdentitySessionDto) {
    this.confirmationService
      .warn('AbpIdentity::SessionRevokeConfirmationMessage', 'AbpUi::AreYouSure')
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() =>
          this.service.revoke(row.id).pipe(
            tap(() => {
              if (row.isCurrent) {
                return this.authService
                  .logout({ noRedirectToLogoutUrl: true })
                  .pipe(switchMap(() => this.configStateService.refreshAppState()))
                  .subscribe();
              }
              this.hookToQuery();
            }),
          ),
        ),
      )
      .subscribe();
  }
}
