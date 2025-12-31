import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AccountService } from '../../services/account.service';
import { SecurityCodeService } from '../../services/security-code.service';

//Todo: @masumulu28 improve to v14+ this component
@Component({
  selector: 'abp-send-security-code',
  templateUrl: 'send-security-code.component.html',
})
export class SendSecurityCodeComponent implements OnInit, OnDestroy {
  providers: string[] = [];
  selectedProvider: string;
  loading: boolean;
  showCodeForm: boolean;

  codeForm = this.fb.group({
    code: [null, [Validators.required]],
  });

  constructor(
    protected service: SecurityCodeService,
    protected accountService: AccountService,
    protected fb: UntypedFormBuilder,
  ) {}

  ngOnInit() {
    const { twoFactorToken: token, userId } = this.service.data;

    this.accountService.getTwoFactorProviders({ token, userId }).subscribe(res => {
      this.providers = res;
      this.selectedProvider = res[0];
    });
  }

  ngOnDestroy() {
    this.service.data = null;
  }

  sendTwoFactorCode() {
    if (this.loading) return;
    this.loading = true;

    const { twoFactorToken: token, userId } = this.service.data;
    this.accountService
      .sendTwoFactorCode({
        token,
        userId,
        provider: this.selectedProvider,
      })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.showCodeForm = true;
      });
  }

  login() {
    if (this.codeForm.invalid) return;

    const { code } = this.codeForm.value;

    this.loading = true;
    this.service
      .login({ code, provider: this.selectedProvider })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }
}
