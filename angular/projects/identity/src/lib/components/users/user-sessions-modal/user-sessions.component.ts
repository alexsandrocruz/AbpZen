import { Component, OnInit, inject, input, model, signal } from '@angular/core';
import { filter, switchMap, tap } from 'rxjs';
import { NgbDropdownModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import {
  AuthService,
  ConfigStateService,
  CoreModule,
  ListService,
  PagedResultDto,
} from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  GetIdentitySessionListInput,
  IdentitySessionDto,
  IdentitySessionService,
  IdentityUserDto,
} from '@volo/abp.ng.identity/proxy';
import { UserSessionDetailComponent } from './session-detail-modal/user-session-detail.component';

@Component({
  standalone: true,
  selector: 'abp-user-sessions',
  templateUrl: './user-sessions.component.html',
  imports: [
    NgbDropdownModule,
    NgbTooltip,
    CoreModule,
    ThemeSharedModule,
    UserSessionDetailComponent,
  ],
  providers: [ListService],
})
export class UserSessionsComponent implements OnInit {
  protected readonly list = inject(ListService<GetIdentitySessionListInput>);
  protected readonly service = inject(IdentitySessionService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly authService = inject(AuthService);
  protected readonly configStateService = inject(ConfigStateService);

  modalOptions = { size: 'xl' };

  isModalVisible = model<boolean>(false);
  selected = input.required<IdentityUserDto>();

  data = signal<PagedResultDto<IdentitySessionDto>>({ items: [], totalCount: 0 });
  sessionId = signal('');
  visibleSessionDetailModal = signal(false);

  protected hookToQuery(): void {
    this.list
      .hookToQuery(query =>
        this.service.getList({
          userId: this.selected().id,
          ...query,
        }),
      )
      .subscribe(this.data.set);
  }

  ngOnInit(): void {
    this.hookToQuery();
  }

  showDetail(row: IdentitySessionDto): void {
    this.visibleSessionDetailModal.set(true);
    this.sessionId.set(row.id);
  }

  revokeSession(row: IdentitySessionDto): void {
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
