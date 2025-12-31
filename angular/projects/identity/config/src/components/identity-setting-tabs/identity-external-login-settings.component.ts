import { Component, inject, OnInit } from '@angular/core';
import { IdentityOAuthSettingsDto, IdentitySettingsService } from '@volo/abp.ng.identity/proxy';
import { ToasterService } from '@abp/ng.theme.shared';
import { ConfigStateService } from '@abp/ng.core';
import { finalize } from 'rxjs/operators';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'abp-external-login-settings',
  templateUrl: './identity-external-login-settings.component.html',
})
export class IdentityExternalLoginSettingsComponent implements OnInit {
  settings: IdentityOAuthSettingsDto;

  loading = false;

  form: UntypedFormGroup;

  protected readonly service = inject(IdentitySettingsService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly fb = inject(UntypedFormBuilder);

  ngOnInit() {
    this.service.getOAuth().subscribe(settings => {
      this.settings = settings;
      this.buildForm();
    });
  }

  submit() {
    if (this.form.invalid) {
      return;
    }
    this.loading = true;
    this.service
      .updateOAuth(this.form.value)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully', null);
        this.configState.refreshAppState().subscribe();
      });
  }

  buildForm() {
    this.form = this.fb.group({
      enableOAuthLogin: [this.settings.enableOAuthLogin, []],
      clientId: [this.settings.clientId, [Validators.required]],
      clientSecret: [this.settings.clientSecret, []],
      authority: [this.settings.authority, [Validators.required]],
      scope: [this.settings.scope, []],
      requireHttpsMetadata: [this.settings.requireHttpsMetadata, []],
      validateIssuerName: [this.settings.validateIssuerName, []],
      validateEndpoints: [this.settings.validateEndpoints, []],
    });
  }
}
