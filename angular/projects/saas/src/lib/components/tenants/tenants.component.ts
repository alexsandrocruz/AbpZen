import { Component, inject, Injector, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { finalize, map, tap } from 'rxjs/operators';
import { NgbDateAdapter, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';

import {
  CoreModule,
  ListService,
  LocalizationModule,
  PagedResultDto,
  ReplaceableComponents,
} from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  ToasterService,
  DateAdapter,
  ThemeSharedModule,
} from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { PageModule } from '@abp/ng.components/page';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  EditionDto,
  EditionLookupDto,
  EditionService,
  GetTenantsInput,
  SaasTenantConnectionStringsDto,
  SaasTenantCreateDto,
  SaasTenantDto,
  tenantActivationStateOptions,
  TenantService,
} from '@volo/abp.ng.saas/proxy';

import {
  SetTenantPasswordModalInputs,
  SetTenantPasswordModalOutputs,
} from './../../models/set-tenant-password-modal.model';
import { eSaasComponents } from '../../enums/components';
import { TENANT_FORM_ASYNC_VALIDATORS_TOKEN } from '../../tokens';
import { ConnectionStringsFormBuilderService } from '../../services';

import { ConnectionStringsComponent } from './connection-strings';
import { ImpersonateTenantModalComponent } from './impersonate-tenant-modal.component';
import { SetTenantPasswordModalComponent } from './set-tenant-password-modal.component';

@Component({
  standalone: true,
  selector: 'abp-tenants',
  templateUrl: './tenants.component.html',
  providers: [
    ListService,
    ConnectionStringsFormBuilderService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eSaasComponents.Tenants,
    },
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
  imports: [
    ReactiveFormsModule,
    NgbNavModule,
    NgbTooltipModule,
    CoreModule,
    LocalizationModule,
    PageModule,
    ThemeSharedModule,
    CommercialUiModule,
    FeatureManagementModule,
    ConnectionStringsComponent,
    ImpersonateTenantModalComponent,
    SetTenantPasswordModalComponent,
  ],
})
export class TenantsComponent implements OnInit {
  protected readonly list = inject(ListService<GetTenantsInput>);
  protected readonly service = inject(TenantService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly fb = inject(UntypedFormBuilder);
  protected readonly injector = inject(Injector);
  protected readonly toasterService = inject(ToasterService);
  protected readonly editionService = inject(EditionService);
  protected readonly conStrFormBuilderService = inject(ConnectionStringsFormBuilderService);

  protected readonly tenantFormAsyncValidators = inject(TENANT_FORM_ASYNC_VALIDATORS_TOKEN);

  data: PagedResultDto<SaasTenantDto> = { items: [], totalCount: 0 };
  editions: EditionDto[];

  selected = signal<SaasTenantDto>({} as SaasTenantDto);

  formFilters: UntypedFormGroup;
  tenantForm: UntypedFormGroup;
  databaseConnectionStringsForm = this.conStrFormBuilderService.generateForm();

  isConnectionStringModalVisible = signal(false);
  isModalVisible: boolean;
  modalTitle: string;
  modalBusy = false;

  visibleFeatures = false;
  isVisibleSetTenantPasswordModal = false;
  selectedTenantId = '';
  selectedTenantNameForSetTenantPasswordModal = '';
  providerKey: string;
  providerTitle: string;

  emptyOption = { label: '-', value: null };

  connectionStringsReplacementKey = eSaasComponents.ConnectionStrings;
  setTenantPasswordReplacementKey = eSaasComponents.SetTenantPassword;
  activationStateOptions = tenantActivationStateOptions;

  setTenantReplaceableTemplateOptions: ReplaceableComponents.ReplaceableTemplateDirectiveInput<
    SetTenantPasswordModalInputs,
    SetTenantPasswordModalOutputs
  > | null = null;

  private buildFilterForm() {
    this.formFilters = this.fb.group({
      getEditionNames: [null, []],
      editionId: [null, []],
      times: [{}],
      activationState: [null, []],
    });
  }

  protected hookToQuery() {
    this.list
      .hookToQuery(query => {
        const value = {
          ...this.formFilters.value,
          ...this.formFilters.value.times,
        };
        return this.service.getList({ ...query, ...value });
      })
      .subscribe(res => (this.data = res));
  }

  protected createTenantForm() {
    return this.editionService.getList({ maxResultCount: 1000 }).pipe(
      tap(res => {
        this.editions = res.items;
        const data = new FormPropData(this.injector, this.selected());
        this.tenantForm = generateFormFromProps(data);

        if (!this.selected()?.id) {
          this.tenantForm = this.fb.group({
            ...this.tenantForm.controls,
            ...this.conStrFormBuilderService.generateForm().controls,
          });
        }

        if (this.tenantFormAsyncValidators) {
          this.tenantForm.addAsyncValidators(this.tenantFormAsyncValidators);
        }

        if (!data.record?.editionId) {
          this.tenantForm.controls.editionId.patchValue(null);
        }
      }),
    );
  }

  ngOnInit() {
    this.buildFilterForm();
    this.hookToQuery();
  }

  getData() {
    if (this.formFilters.invalid && this.formFilters.touched) {
      return;
    }
    this.list.get();
  }

  clearFilters() {
    this.formFilters.reset({ times: {} });
  }

  openModal(title: string) {
    this.modalTitle = title;
    this.isModalVisible = true;
  }

  onAddTenant() {
    this.selected.set({} as SaasTenantDto);
    this.createTenantForm().subscribe(() => {
      this.openModal('Saas::NewTenant');
    });
  }

  onEditTenant(id: string) {
    this.service.get(id).subscribe(tenant => {
      this.selected.set(tenant);
      this.createTenantForm().subscribe(() => this.openModal('Saas::Edit'));
    });
  }

  save() {
    if (!this.tenantForm.valid || this.modalBusy) {
      return;
    }

    this.modalBusy = true;

    const value = this.tenantForm.value as SaasTenantCreateDto;

    const { id } = this.selected() || {};

    let request = this.service.create(value);
    if (id) {
      request = this.service.update(id, { ...this.selected(), ...value });
    }

    request.pipe(finalize(() => (this.modalBusy = false))).subscribe(() => {
      this.list.get();
      this.isModalVisible = false;
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }

  saveConectionStrings() {
    if (this.databaseConnectionStringsForm.invalid) {
      return;
    }

    const input = this.databaseConnectionStringsForm.value
      .connectionStrings as SaasTenantConnectionStringsDto;

    this.service.updateConnectionStrings(this.selected().id, input).subscribe(() => {
      this.isConnectionStringModalVisible.set(false);
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('Saas::TenantDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(id).subscribe(() => this.list.get());
          this.toasterService.success('AbpUi::DeletedSuccessfully', '', { life: 6000 });
        }
      });
  }

  openFeaturesModal(providerKey: string, name: string) {
    this.providerKey = providerKey;
    this.providerTitle = name;
    setTimeout(() => {
      this.visibleFeatures = true;
    }, 0);
  }

  openSetTenantPasswordModal(id: string, name: string) {
    this.isVisibleSetTenantPasswordModal = true;
    this.selectedTenantNameForSetTenantPasswordModal = name;
    this.selectedTenantId = id;

    this.setTenantReplaceableTemplateOptions = {
      outputs: { modalVisibleChange: this.closeSetTenantPasswordModal },
      inputs: {
        modalVisible: { value: this.isVisibleSetTenantPasswordModal },
        tenantId: { value: this.selectedTenantId },
        tenantName: { value: this.selectedTenantNameForSetTenantPasswordModal },
      },
      componentKey: this.setTenantPasswordReplacementKey,
    };
  }

  closeSetTenantPasswordModal = () => {
    this.isVisibleSetTenantPasswordModal = false;
    this.selectedTenantNameForSetTenantPasswordModal = '';
    this.selectedTenantId = '';
    this.setTenantReplaceableTemplateOptions = null;
  };

  applyDatabaseMigrations(recordId: string) {
    this.service.applyDatabaseMigrations(recordId).subscribe(() =>
      this.toasterService.info('Saas::DatabaseMigrationQueuedAndWillBeApplied', '', {
        life: 6000,
      }),
    );
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };

  getEditionLookup() {
    return () =>
      this.service
        .getEditionLookup()
        .pipe(map(items => ({ items }) as PagedResultDto<EditionLookupDto>));
  }
}
