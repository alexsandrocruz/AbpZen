import { Component, ChangeDetectionStrategy } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { AbstractAuditLogSettingsComponent } from '../../../../abstracts';

@Component({
  standalone: true,
  selector: 'abp-audit-log-settings-global',
  templateUrl: './audit-log-settings-global.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [AsyncPipe, ReactiveFormsModule, LocalizationModule, ThemeSharedModule, DatePipe],
})
export class AuditLogSettingsGlobalComponent extends AbstractAuditLogSettingsComponent {
  globalTab = true;
}
