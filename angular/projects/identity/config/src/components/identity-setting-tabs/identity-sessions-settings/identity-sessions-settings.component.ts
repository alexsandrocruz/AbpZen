import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { LocalizationModule } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import {
  IdentitySessionSettingsDto,
  IdentitySettingsService,
  Settings,
} from '@volo/abp.ng.identity/proxy';

@Component({
  standalone: true,
  selector: 'abp-identity-session-setting',
  templateUrl: './identity-sessions-settings.component.html',
  imports: [ReactiveFormsModule, LocalizationModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IdentitySessionsSettingComponent {
  protected readonly fb = inject(FormBuilder);
  protected readonly identitySettingService = inject(IdentitySettingsService);
  protected readonly toasterService = inject(ToasterService);

  sessionSettings = Settings.identityProPreventConcurrentLoginBehaviourOptions;
  form = this.fb.group({ preventConcurrentLogin: [] });

  protected buildForm(): void {
    this.identitySettingService.getSession().subscribe(({ preventConcurrentLogin }) => {
      this.form.setValue({ preventConcurrentLogin });
    });
  }

  constructor() {
    this.buildForm();
  }

  save(): void {
    this.identitySettingService
      .updateSession(this.form.value as IdentitySessionSettingsDto)
      .subscribe(() => this.toasterService.success('AbpUi::SavedSuccessfully'));
  }
}
