import { Component, inject, OnInit } from '@angular/core';
import { IdentityLdapSettingsDto, IdentitySettingsService } from '@volo/abp.ng.identity/proxy';
import { finalize } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'abp-ldap-login-settings',
  templateUrl: './identity-ldap-settings.component.html',
})
export class IdentityLdapSettingsComponent implements OnInit {
  settings: IdentityLdapSettingsDto;

  loading = false;

  protected readonly service = inject(IdentitySettingsService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly configState = inject(ConfigStateService);

  ngOnInit() {
    this.service.getLdap().subscribe(settings => (this.settings = settings));
  }

  submit(settings: IdentityLdapSettingsDto) {
    this.loading = true;
    this.service
      .updateLdap(settings)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully', null);
        this.configState.refreshAppState().subscribe();
      });
  }
}
