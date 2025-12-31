import { ChangeDetectionStrategy, Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { map } from 'rxjs/operators';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule, ListResultDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { ThemeSharedModule, DateTimeAdapter } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  DelegateNewUserInput,
  GetUserLookupInput,
  IdentityUserDelegationService,
  UserLookupDto,
} from '@volo/abp.ng.account/public/proxy';
import { timeRangeValidator } from '../../validators';
import { lastValueFrom } from 'rxjs';

const TIME_FIELDS = ['startTime', 'endTime'];

@Component({
  standalone: true,
  selector: 'abp-create-user-delegate',
  templateUrl: './create-user-delegate.component.html',
  imports: [CoreModule, ThemeSharedModule, CommercialUiModule],
  providers: [{ provide: NgbDateAdapter, useClass: DateTimeAdapter }],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateUserDelegateComponent {
  protected readonly identityUserDelegationService = inject(IdentityUserDelegationService);
  protected readonly fb = inject(FormBuilder);

  modalVisible = true;

  form = this.fb.group({
    userNameId: ['', [Validators.required]],
    times: [
      {
        startTime: '',
        endTime: '',
      },
      [timeRangeValidator(TIME_FIELDS)],
    ],
  });

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() save = new EventEmitter();

  getFn = (input: PagedAndSortedResultRequestDto & { filter: string }) => {
    return this.identityUserDelegationService
      .getUserLookup({ userName: input.filter } as GetUserLookupInput)
      .pipe(
        map((usersLookup: ListResultDto<UserLookupDto>) => {
          return {
            items: usersLookup.items,
            totalCount: usersLookup.items.length,
          };
        }),
      );
  };

  async submit() {
    if (this.form.invalid) {
      return;
    }

    const { userNameId, times } = this.form.value;
    const { startTime, endTime } = times || {};

    const input: DelegateNewUserInput = {
      targetUserId: userNameId,
      startTime,
      endTime,
    };

    await lastValueFrom(this.identityUserDelegationService.delegateNewUser(input));
    this.save.emit();
    this.visibleChange.emit(false);
  }
}
