import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { TenantBoxService } from '@volo/abp.ng.account.core';

@Component({
  standalone: true,
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
  imports: [CoreModule, ThemeSharedModule, AsyncPipe],
  providers: [TenantBoxService],
})
export class TenantBoxComponent {
  protected readonly service = inject(TenantBoxService);
}
