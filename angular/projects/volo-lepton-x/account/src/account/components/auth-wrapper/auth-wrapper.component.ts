import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { AuthWrapperService } from '@volo/abp.ng.account.core';
import { TenantBoxComponent } from '../tenant-box/tenant-box.component';

@Component({
  standalone: true,
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  imports: [AsyncPipe, NgbDropdownModule, CoreModule, TenantBoxComponent],
  providers: [AuthWrapperService],
})
export class AuthWrapperComponent {
  protected readonly service = inject(AuthWrapperService);
}
