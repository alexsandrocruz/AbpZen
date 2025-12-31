import {
  ListService,
  LocalizationModule,
  PagedResultDto,
  ReplaceableTemplateDirective,
} from '@abp/ng.core';
import { ConfirmationService, ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { PageModule } from '@abp/ng.components/page';
import { EditionDto, EditionService, GetEditionsInput, Payment } from '@volo/abp.ng.saas/proxy';
import { AdvancedEntityFiltersModule } from '@volo/abp.commercial.ng.ui';
import { Component, DestroyRef, Injector, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { finalize } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { EditionDeleteComponent } from './edition-delete.component';
import { EditionVisibleChange } from './edition-edit.modal';
import { MoveAllTenantsComponent } from './move-all-tenants.component';
import { eSaasComponents } from '../../enums/components';
@Component({
  selector: 'abp-editions',
  templateUrl: './editions.component.html',
  standalone: true,
  imports: [
    EditionDeleteComponent,
    MoveAllTenantsComponent,
    NgxValidateCoreModule,
    PageModule,
    ThemeSharedModule,
    AdvancedEntityFiltersModule,
    FeatureManagementModule,
    ExtensibleModule,
    ReplaceableTemplateDirective,
    LocalizationModule,
    ReactiveFormsModule,
  ],
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eSaasComponents.Editions,
    },
  ],
})
export class EditionsComponent implements OnInit {
  public readonly list = inject(ListService<GetEditionsInput>);
  protected readonly toasterService = inject(ToasterService);
  protected readonly service = inject(EditionService);
  protected readonly injector = inject(Injector);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly destroyRef = inject(DestroyRef);

  data: PagedResultDto<EditionDto> = { items: [], totalCount: 0 };

  plans$ = new BehaviorSubject<Payment.Plans.PlanDto[]>([]);
  selected: EditionDto;
  editionForm: UntypedFormGroup;
  visibleMove = false;
  isModalVisible = false;
  visibleEditionDelete = false;
  visibleFeatures = false;
  providerKey: string;
  providerTitle: string;
  modalBusy = false;

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }

  createEditionForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.editionForm = generateFormFromProps(data);
    this.getPlanLookup();
  }

  onAddEdition() {
    this.selected = {} as EditionDto;
    this.createEditionForm();
    this.isModalVisible = true;
  }

  onEditEdition(id: string) {
    this.service.get(id).subscribe(selectedEdition => {
      this.selected = selectedEdition;
      this.createEditionForm();
      this.isModalVisible = true;
    });
  }

  save() {
    if (!this.editionForm.valid) return;
    this.modalBusy = true;

    const { id } = this.selected;

    (id
      ? this.service.update(id, { ...this.selected, ...this.editionForm.value })
      : this.service.create(this.editionForm.value)
    )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.list.get();
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.isModalVisible = false;
      });
  }

  openFeaturesModal(providerKey: string, providerTitle: string) {
    this.providerKey = providerKey;
    this.providerTitle = providerTitle;
    setTimeout(() => {
      this.visibleFeatures = true;
    }, 0);
  }

  getPlanLookup() {
    this.service.getPlanLookup().subscribe(items => {
      this.plans$.next(items);
    });
  }

  delete(edition: EditionDto) {
    this.selected = edition;
    this.visibleEditionDelete = true;
  }

  onVisibleDeleteChange(v: EditionVisibleChange) {
    if (v.visible) {
      return;
    }
    if (v.refresh) {
      this.list.get();
    }
    this.selected = null;
    this.visibleEditionDelete = false;
  }

  moveAllTenants(edition: EditionDto) {
    if (edition.tenantCount === 0) {
      this.confirmationService
        .warn('Saas::ThereIsNoTenantsCurrentlyInThisEdition', 'AbpUi::Warning', {
          hideCancelBtn: true,
          yesText: 'AbpUi::Ok',
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe();
      return;
    }
    this.selected = edition;
    this.visibleMove = true;
  }

  onVisibleMoveChange(v: EditionVisibleChange) {
    if (v.visible) {
      return;
    }
    if (v.refresh) {
      this.list.get();
    }
    this.selected = null;
    this.visibleMove = false;
  }
}
