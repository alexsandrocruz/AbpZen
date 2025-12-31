import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, interval, map, Subject, switchMap, takeUntil, tap } from 'rxjs';

import { ConfigStateService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { AccountService } from '@volo/abp.ng.account/public/proxy';

import { ConfirmUserParams } from '../models';

export const REQUIRE_EMAIL_SETTING_KEY = 'Abp.Identity.SignIn.RequireConfirmedEmail';
export const REQUIRE_PHONE_SETTING_KEY = 'Abp.Identity.SignIn.RequireConfirmedPhoneNumber';

@Injectable()
export class ConfirmUserService {
  protected readonly destroy = new Subject<void>();
  protected readonly router = inject(Router);
  protected readonly configService = inject(ConfigStateService);
  protected readonly accountService = inject(AccountService);
  protected readonly toasterService = inject(ToasterService);

  readonly #data = signal<Partial<ConfirmUserParams>>({ email: { showButton: true } });

  userId = computed(() => this.#data()?.userId);
  email = computed(() => this.#data()?.email);
  phone = computed(() => this.#data()?.phone);

  protected isSettingTrueMap() {
    return map(val => val === 'True');
  }

  confirmationStateRequest$() {
    return this.accountService.getConfirmationState(this.userId());
  }

  initialize() {
    const emailSetting$ = this.configService
      .getSetting$(REQUIRE_EMAIL_SETTING_KEY)
      .pipe(this.isSettingTrueMap());

    const phoneSetting$ = this.configService
      .getSetting$(REQUIRE_PHONE_SETTING_KEY)
      .pipe(this.isSettingTrueMap());

    const confirmationState$ = this.confirmationStateRequest$();

    combineLatest([emailSetting$, phoneSetting$, confirmationState$]).subscribe(
      ([emailSetting, phoneSetting, { emailConfirmed, phoneNumberConfirmed }]) => {
        this.#data.update(prev => ({
          ...prev,
          email: {
            ...prev.email,
            requireSetting: emailSetting,
            showButton: emailSetting && !emailConfirmed,
            confirmed: emailConfirmed,
          },
          phone: {
            ...prev.phone,
            requireSetting: phoneSetting,
            showButton: phoneSetting && !phoneNumberConfirmed,
            confirmed: phoneNumberConfirmed,
          },
        }));
      },
    );
  }

  update(updateFn: (prev) => Partial<ConfirmUserParams>) {
    this.#data.update(updateFn);
  }

  updateConfirmedState(emailConfirmed: boolean, phoneNumberConfirmed: boolean) {
    this.#data.update(prev => ({
      ...prev,
      email: {
        ...prev.email,
        confirmed: emailConfirmed,
      },
      phone: {
        ...prev.phone,
        confirmed: phoneNumberConfirmed,
      },
    }));
  }

  validateUserId(): boolean {
    if (!this.userId()) {
      this.router.navigate(['/account/login']);
      return false;
    }

    return true;
  }

  sendEmailConfirmation() {
    const input = {
      appName: 'Angular',
      userId: this.userId(),
    };

    this.accountService.sendEmailConfirmationToken(input).subscribe(() => {
      this.toasterService.success('AbpAccount::EmailConfirmationSentMessage', '', {
        messageLocalizationParams: [this.#data().email.address],
      });

      this.#data.update(prev => ({ ...prev, email: { ...prev.email, showButton: false } }));
    });

    const interval$ = interval(3000).pipe(
      switchMap(() => this.confirmationStateRequest$()),
      tap(({ emailConfirmed, phoneNumberConfirmed }) => {
        if (phoneNumberConfirmed && emailConfirmed) {
          this.router.navigate(['/account/login']);
        } else {
          this.updateConfirmedState(emailConfirmed, phoneNumberConfirmed);
        }

        if (emailConfirmed) {
          this.destroy.next();
        }
      }),
      takeUntil(this.destroy),
    );

    interval$.subscribe();
  }

  sendPhoneConfirmation(phoneNumber: string) {
    if (!phoneNumber) {
      return;
    }

    const input = {
      userId: this.userId(),
      phoneNumber,
    };

    this.accountService.sendPhoneNumberConfirmationToken(input).subscribe();
  }

  verifyPhoneCode(token: string) {
    this.accountService
      .confirmPhoneNumber({
        userId: this.userId(),
        token,
      })
      .pipe(
        switchMap(() => this.confirmationStateRequest$()),
        tap(({ emailConfirmed, phoneNumberConfirmed }) =>
          this.updateConfirmedState(emailConfirmed, phoneNumberConfirmed),
        ),
      )
      .subscribe(({ emailConfirmed }) => {
        if (emailConfirmed) {
          this.router.navigate(['/account/login']);
        }
      });
  }
}
