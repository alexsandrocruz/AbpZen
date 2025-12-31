import { ConfigStateService } from '@abp/ng.core';
import { Injectable, Injector, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { mapTo, switchMap } from 'rxjs/operators';
import {
  RecaptchaPageStrategy,
  RecaptchaStrategy,
  RecaptchaV2Strategy,
  RecaptchaV3Strategy,
} from '../strategies/recaptcha.strategy';
import { AccountService } from './account.service';

@Injectable()
export class RecaptchaService implements OnDestroy {
  protected pageStrategy: RecaptchaPageStrategy;
  protected strategy: RecaptchaStrategy;

  get isEnabled(): boolean {
    return this.pageStrategy.isEnabled;
  }

  protected get reCaptchaVersion() {
    return this.configState.getSetting('Abp.Account.Captcha.Version');
  }

  constructor(
    protected injector: Injector,
    protected configState: ConfigStateService,
    protected accountService: AccountService,
  ) {}

  setStrategy(strategy: RecaptchaPageStrategy) {
    this.pageStrategy = strategy;
    if (!this.pageStrategy.isEnabled) return;
    this.init();
  }

  protected init() {
    if (this.reCaptchaVersion === '2')
      this.strategy = new RecaptchaV2Strategy(this.injector, this.pageStrategy.targetElement);
    else if (this.reCaptchaVersion === '3') {
      this.strategy = new RecaptchaV3Strategy(this.injector, this.pageStrategy.action);
    }
  }

  validate(): Observable<any> {
    return this.strategy
      .getVerificationToken()
      .pipe(
        switchMap(token => this.accountService.recaptchaByCaptchaResponse(token).pipe(mapTo(true))),
      );
  }

  getVerificationToken() {
    return this.strategy.getVerificationToken();
  }

  ngOnDestroy() {
    if (!this.pageStrategy.isEnabled) return;

    this.strategy.destroy();
  }

  reset() {
    if (!this.isEnabled) return;
    this.strategy.reset();
  }
}
