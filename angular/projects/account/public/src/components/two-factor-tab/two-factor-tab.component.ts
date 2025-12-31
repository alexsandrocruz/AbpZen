import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder } from '@angular/forms';
import { EMPTY, catchError, finalize, skip, switchMap } from 'rxjs';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { TwoFactorTabService } from '../../services';

@Component({
  standalone: true,
  selector: 'abp-two-factor-tab',
  templateUrl: './two-factor-tab.component.html',
  imports: [CoreModule, ThemeSharedModule],
  providers: [TwoFactorTabService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TwoFactorTabComponent {
  protected readonly fb = inject(FormBuilder);
  protected readonly twoFactorTabService = inject(TwoFactorTabService);

  readonly form = this.fb.group({ isTwoFactorEnabled: [false] });
  isLoaded = signal(false);

  protected isTwoFactorEnabled(): void {
    this.isLoaded.set(false);
    this.twoFactorTabService.isTwoFactorEnabled$
      .pipe(finalize(() => this.isLoaded.set(true)))
      .subscribe(value => this.form.controls.isTwoFactorEnabled.setValue(value));
  }

  protected trackTwoFactorStatus(): void {
    this.form.valueChanges
      .pipe(
        skip(1),
        switchMap(({ isTwoFactorEnabled }) =>
          this.twoFactorTabService.setTwoFactorStatus(isTwoFactorEnabled).pipe(
            catchError(() => {
              this.form.controls.isTwoFactorEnabled.setValue(false, { emitEvent: false });
              return EMPTY;
            }),
          ),
        ),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  constructor() {
    this.isTwoFactorEnabled();
    this.trackTwoFactorStatus();
  }
}
