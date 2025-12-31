import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { filter, switchMap } from 'rxjs';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule, AbpWindowService } from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  ThemeSharedModule,
  ToasterService,
} from '@abp/ng.theme.shared';
import { QrCodeComponent } from '../qr-code/qr-code.component';
import { AuthenticatorAppService } from '../../services/authenticator-app.service';
import { AuthenticatorSteps } from '../../models';

@Component({
  standalone: true,
  selector: 'abp-authenticator-app',
  templateUrl: './authenticator-app.component.html',
  providers: [AuthenticatorAppService],
  imports: [NgbNavModule, CoreModule, ThemeSharedModule, QrCodeComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthenticatorAppComponent {
  protected readonly authenticatorAppService = inject(AuthenticatorAppService);
  protected readonly windowService = inject(AbpWindowService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly confirmationService = inject(ConfirmationService);

  activeStep: AuthenticatorSteps = 'Authenticator';
  verifyCode = '';
  isVerifiedAuthenticatorCode = signal(false);
  hasAuthenticator = this.authenticatorAppService.hasAuthenticator;
  authenticatorInfo = this.authenticatorAppService.authenticatorInfo;
  recoveryCodeList = signal<string[]>([]);
  hasRecoveryCodes = computed(() => this.recoveryCodeList().length > 0);

  copyToClipboard(text: string): void {
    this.windowService.copyToClipboard(text);
    this.toasterService.success('AbpUi::CopiedToTheClipboard');
  }

  verifyQrCode(): void {
    if (!this.verifyCode) {
      return;
    }

    this.authenticatorAppService
      .verifyAuthenticatorCode(this.verifyCode)
      .subscribe(({ recoveryCodes }) => {
        this.isVerifiedAuthenticatorCode.set(true);
        this.recoveryCodeList.set(recoveryCodes);
      });
  }

  printRecoverCodes(): void {
    if (this.recoveryCodeList().length < 1) return;

    const printWindow = this.windowService.open();
    printWindow.document.write('<html><body>');
    printWindow.document.write(...this.recoveryCodeList().join('<br>'));
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
  }

  recoveryCodeOk(): void {
    this.windowService.reloadPage();
  }

  copySharedKey(): void {
    this.copyToClipboard(this.authenticatorInfo().key);
  }

  copyRecoveryCodes(): void {
    const codeText = this.recoveryCodeList().join('\n');
    this.copyToClipboard(codeText);
  }

  resetAuthenticator(): void {
    this.confirmationService
      .warn('AbpAccount::ResetAuthenticatorWarningMessage', 'AbpUi::AreYouSure')
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.authenticatorAppService.resetAuthenticator()),
      )
      .subscribe(() => this.windowService.reloadPage());
  }
}
