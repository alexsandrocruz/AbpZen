import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { QRCodeModule } from 'angularx-qrcode';
import { QRCodeErrorCorrectionLevel } from 'qrcode';
import { CoreModule } from '@abp/ng.core';

@Component({
  standalone: true,
  selector: 'abp-qr-code',
  template: `
    @if (qrData; as data) {
      <qrcode
        [qrdata]="data"
        [width]="width || 256"
        [errorCorrectionLevel]="errorCorrectionLevel || 'M'"
      ></qrcode>
    } @else {
      <div class="text-center text-muted">
        <i class="fa fa-qrcode fa-5x"></i>
      </div>
    }
  `,
  imports: [QRCodeModule, CoreModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class QrCodeComponent {
  @Input() qrData: string | undefined;
  @Input() width: number | undefined;
  @Input() errorCorrectionLevel: QRCodeErrorCorrectionLevel | undefined;
}
