import { Component, ChangeDetectionStrategy } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { AbstractAuditLogSettingsComponent } from '../../../../abstracts';

@Component({
  standalone: true,
  selector: 'abp-audit-log-settings-general',
  templateUrl: './audit-log-settings-general.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [AsyncPipe, ReactiveFormsModule, LocalizationModule, ThemeSharedModule, DatePipe],
})
export class AuditLogSettingsGeneralComponent extends AbstractAuditLogSettingsComponent {}
