import { ChangeDetectionStrategy, Component, inject, model } from '@angular/core';
import {
  AbstractControl,
  ControlContainer,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { LocalizationModule } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { TenantService } from '@volo/abp.ng.saas/proxy';
import { parentFormGroupProvider } from '../../../providers';

@Component({
  standalone: true,
  selector: 'abp-connection-strings-form-body',
  templateUrl: './connection-strings-form-body.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, ReactiveFormsModule, NgxValidateCoreModule, LocalizationModule],
  viewProviders: [parentFormGroupProvider],
})
export class ConnectionStringsFormBodyComponent {
  protected readonly controlContainer = inject(ControlContainer);
  protected readonly toasterService = inject(ToasterService);
  protected readonly service = inject(TenantService);

  protected get group(): FormGroup {
    return <FormGroup>this.controlContainer.control;
  }

  protected get defaultControl(): AbstractControl {
    return this.group.get('default');
  }

  useSharedDatabase = model(true);

  checkConnectionString(dbConnection?: string) {
    if (!dbConnection && !this.defaultControl.value) {
      return this.toasterService.error('Saas::InvalidConnectionString');
    }

    this.service
      .checkConnectionString(dbConnection || this.defaultControl.value)
      .subscribe(response => {
        if (response) {
          return this.toasterService.success('Saas::ValidConnectionString');
        }

        this.toasterService.error('Saas::InvalidConnectionString');
      });
  }
}
