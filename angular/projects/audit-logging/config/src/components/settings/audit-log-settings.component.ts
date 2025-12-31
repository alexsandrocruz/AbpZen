import { Component, inject } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationModule, SessionStateService } from '@abp/ng.core';
import { AuditLogSettingsGeneralComponent, AuditLogSettingsGlobalComponent } from './tabs';

@Component({
  standalone: true,
  selector: 'abp-audit-log-settings',
  templateUrl: './audit-log-settings.component.html',
  imports: [
    NgbNavModule,
    LocalizationModule,
    AuditLogSettingsGeneralComponent,
    AuditLogSettingsGlobalComponent,
  ],
})
export class AuditLogSettingsComponent {
  protected readonly sessionStateService = inject(SessionStateService);

  isTenant = this.sessionStateService.getTenant()?.isAvailable;
}
