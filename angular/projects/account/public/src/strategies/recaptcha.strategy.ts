import { ConfigStateService, LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';
import { Injector, NgZone } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';

declare const grecaptcha: any;

declare global {
  interface Window {
    recaptchaV2Loaded: () => void;
  }
}

export abstract class RecaptchaPageStrategy {
  abstract action: string;
  abstract settingKey: string;
  protected configState: ConfigStateService;
  targetElement: HTMLElement;
  isEnabled: boolean;

  constructor(configState: ConfigStateService, targetElement: HTMLElement) {
    this.configState = configState;
    this.targetElement = targetElement;
  }

  protected setIsEnabled() {
    this.isEnabled = (this.configState.getSetting(this.settingKey) || '').toLowerCase() === 'true';
  }
}

export class RecaptchaLoginStrategy extends RecaptchaPageStrategy {
  action = 'login';
  settingKey = 'Abp.Account.Captcha.UseCaptchaOnLogin';

  constructor(configState: ConfigStateService, targetElement: HTMLElement) {
    super(configState, targetElement);
    this.setIsEnabled();
  }
}

export class RecaptchaRegisterStrategy extends RecaptchaPageStrategy {
  action = 'register';
  settingKey = 'Abp.Account.Captcha.UseCaptchaOnRegistration';

  constructor(configState: ConfigStateService, targetElement: HTMLElement) {
    super(configState, targetElement);
    this.setIsEnabled();
  }
}

export const RECAPTCHA_STRATEGY = {
  Login(configState: ConfigStateService, targetElement: HTMLElement) {
    return new RecaptchaLoginStrategy(configState, targetElement);
  },
  Register(configState: ConfigStateService, targetElement: HTMLElement) {
    return new RecaptchaRegisterStrategy(configState, targetElement);
  },
};

export abstract class RecaptchaStrategy {
  protected abstract scriptPath: string;
  abstract getVerificationToken(): Observable<string>;
  protected injector: Injector;
  protected lazyLoadService: LazyLoadService;
  protected configState: ConfigStateService;
  protected insertedElement: ConfigStateService;

  protected get cultureName() {
    return this.configState.getAll().localization.currentCulture.cultureName;
  }

  protected get siteKey() {
    return this.configState.getSetting('Abp.Account.Captcha.SiteKey');
  }

  protected get verifyBaseUrl() {
    return this.configState.getSetting('Abp.Account.Captcha.VerifyBaseUrl');
  }

  constructor(injector: Injector) {
    this.injector = injector;
    this.lazyLoadService = injector.get(LazyLoadService);
    this.configState = injector.get(ConfigStateService);
    this.appendScript();
  }

  appendScript() {
    this.lazyLoadService.load(LOADING_STRATEGY.AppendScriptToBody(this.scriptPath)).subscribe();
  }

  destroy() {
    if (!this.scriptPath) return;
    this.lazyLoadService.remove(this.scriptPath);
  }

  reset() {}
}

export class RecaptchaV2Strategy extends RecaptchaStrategy {
  targetElement: HTMLElement;
  zone = this.injector.get(NgZone);
  scriptPath: string;
  protected token = new BehaviorSubject<string>(null);

  constructor(injector: Injector, targetElement: HTMLElement) {
    super(injector);
    this.targetElement = targetElement;
  }

  appendScript() {
    window.recaptchaV2Loaded = () => {
      grecaptcha.render(this.targetElement, {
        sitekey: this.siteKey,
        'expired-callback': () => {
          this.zone.run(() => this.token.next(null));
        },
        callback: response => this.token.next(response),
      });
    };

    this.scriptPath = `${this.verifyBaseUrl}recaptcha/api.js?render=explicit&hl=${this.cultureName}&onload=recaptchaV2Loaded`;

    super.appendScript();
  }

  reset() {
    grecaptcha.reset();
  }

  getVerificationToken() {
    return this.token.asObservable();
  }
}

export class RecaptchaV3Strategy extends RecaptchaStrategy {
  action: string;
  scriptPath: string;

  constructor(injector: Injector, action: string) {
    super(injector);
    this.action = action;
  }

  appendScript() {
    this.scriptPath = `${this.verifyBaseUrl}recaptcha/api.js?hl=${this.cultureName}&render=${this.siteKey}`;

    super.appendScript();
  }

  getVerificationToken(): Observable<string> {
    try {
      // tslint:disable-next-line
      grecaptcha;
    } catch (error) {
      return of(null);
    }

    return new Observable(observer => {
      grecaptcha.ready(() => {
        grecaptcha.execute(this.siteKey, { action: this.action }).then(token => {
          observer.next(token);
        });
      });
    });
  }

  destroy() {
    super.destroy();

    const element = document.querySelector('.grecaptcha-badge');
    element.parentElement.removeChild(element);
  }
}
