import { AsyncPipe } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { SaasTenantDto } from '@volo/abp.ng.saas/proxy';
import { ImpersonationService } from '@volo/abp.commercial.ng.ui/config';
import { LocalizationModule, SubscriptionService } from '@abp/ng.core';
import { ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { ImpersonateTenantModalService } from '../../services/impersonate-tenant-modal.service';
import { ImpersonateTenantModalFormGroupModel } from '../../models/impersonate-tenant.model';

@Component({
  standalone: true,
  selector: 'abp-impersonate-tenant-modal',
  templateUrl: './impersonate-tenant-modal.component.html',
  providers: [SubscriptionService],
  imports: [AsyncPipe, ReactiveFormsModule, LocalizationModule, ThemeSharedModule],
})
export class ImpersonateTenantModalComponent {
  protected readonly service = inject(ImpersonateTenantModalService);
  protected readonly fb = inject(FormBuilder);
  protected readonly impersonationService = inject(ImpersonationService);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly confirmation = inject(ConfirmationService);

  private readonly defaultTenantUserName = 'admin';

  form: FormGroup<ImpersonateTenantModalFormGroupModel>;

  selected = input<SaasTenantDto>();

  get isVisible$() {
    return this.service.isVisible$;
  }

  constructor() {
    this.buildForm();
  }

  private success() {
    this.reset();
  }

  private reset() {
    this.service.hide();
    this.form.reset({ tenantUserName: this.defaultTenantUserName });
  }

  private buildForm() {
    this.form = this.fb.group({
      tenantUserName: [this.defaultTenantUserName, [Validators.required]],
    });
  }

  private error(response: HttpErrorResponse) {
    const title = response.error.error;
    const message = response.error.error_description;
    this.subscription.addOne(
      this.confirmation.error(message, title, {
        hideCancelBtn: true,
        yesText: 'AbpUi::Ok',
      }),
    );
    console.error(response.error);
  }

  visibleChange(val: boolean) {
    if (val === undefined || val) {
      return;
    }

    this.service.setValue(val);
    this.reset();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const userName = this.form.value.tenantUserName;
    const tenantId = this.service.tenantId;
    const sub = this.impersonationService.impersonateTenant(tenantId, userName);
    this.subscription.addOne(
      sub,
      () => {
        this.success();
      },
      error => {
        this.error(error);
      },
    );
  }
}
