import { Component, effect, inject, input, model, signal, OnInit } from '@angular/core';
import {
  ControlContainer,
  FormArray,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationModule } from '@abp/ng.core';
import {
  SaasTenantDatabaseConnectionStringsDto,
  SaasTenantDto,
  TenantService,
} from '@volo/abp.ng.saas/proxy';

import { parentFormGroupProvider } from '../../../providers';
import { ConnectionStringsFormBodyComponent } from './connection-strings-form-body.component';
import { ModuleSpecificDbComponent } from './module-specific-db.component';
import { ConnectionStringsFormBuilderService } from '../../../services';

@Component({
  standalone: true,
  selector: 'abp-connection-strings',
  templateUrl: 'connection-strings.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgbPopoverModule,
    LocalizationModule,
    ConnectionStringsFormBodyComponent,
    ModuleSpecificDbComponent,
  ],
  viewProviders: [parentFormGroupProvider],
})
export class ConnectionStringsComponent implements OnInit {
  protected readonly service = inject(TenantService);
  protected readonly controlContainer = inject(ControlContainer);
  protected readonly conStrBuilderService = inject(ConnectionStringsFormBuilderService);

  loading: boolean;
  selectedDatabase: SaasTenantDatabaseConnectionStringsDto;

  useSharedDatabase = signal(true);

  moduleSpecificDatabase = model(false);

  selected = input<SaasTenantDto>();

  get connectionStringGroup(): FormGroup {
    return this.controlContainer.control as FormGroup;
  }

  constructor() {
    effect(
      () => {
        const group = this.connectionStringGroup?.get('connectionStrings') as FormGroup;
        const control = group.get('default');
        const dbsControl = group.get('databases') as FormArray;

        if (this.useSharedDatabase()) {
          control?.setErrors(null);
          control.removeValidators(Validators.required);
          control?.setValue('');
          dbsControl.clear({ emitEvent: true });
          this.moduleSpecificDatabase.set(false);
          return;
        }

        if (!this.moduleSpecificDatabase()) {
          dbsControl.clear({ emitEvent: true });
        }

        if (!this.useSharedDatabase() && !control?.value) {
          control.setValidators(Validators.required);
        }
      },
      { allowSignalWrites: true },
    );
  }

  ngOnInit(): void {
    if (this.selected()?.id) {
      this.useSharedDatabase.set(false);
    }
  }
}
