import { MultiTenancyService } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';

export interface EmailConfirmationInput {
  confirmationToken: string;
  userId: string;
}

@Component({
  selector: 'abp-email-confirmation',
  template: `
    @if (confirmed) {
      <p>
        {{ 'AbpAccount::YourEmailAddressIsSuccessfullyConfirmed' | abpLocalization }}
      </p>
    }

    @if (notValid) {
      <p class="text-danger font-weight-bold">
        {{ 'AbpUi::ValidationErrorMessage' | abpLocalization }}
      </p>
    }

    <a role="button" class="btn btn-primary" [routerLink]="['/account/login']">{{
      'AbpAccount::Login' | abpLocalization
    }}</a>
  `,
})
export class EmailConfirmationComponent implements OnInit, OnDestroy {
  confirmed: boolean;

  notValid: boolean;

  constructor(
    private multiTenancy: MultiTenancyService,
    private accountService: AccountService,
    private route: ActivatedRoute,
  ) {}

  private resetTenantBox = () => {};

  ngOnInit() {
    const { isTenantBoxVisible } = this.multiTenancy;
    this.resetTenantBox = () => (this.multiTenancy.isTenantBoxVisible = isTenantBoxVisible);

    // throws ExpressionChangedAfterItHasBeenCheckedError without setTimeout
    setTimeout(() => (this.multiTenancy.isTenantBoxVisible = false), 0);

    this.confirmation();
  }

  ngOnDestroy() {
    this.resetTenantBox();
  }

  confirmation() {
    const { userId, confirmationToken: token } = this.route.snapshot
      .queryParams as EmailConfirmationInput;

    if (!userId || !token) {
      this.notValid = true;
      return;
    }

    this.accountService.confirmEmail({ userId, token }).subscribe(() => (this.confirmed = true));
  }
}
