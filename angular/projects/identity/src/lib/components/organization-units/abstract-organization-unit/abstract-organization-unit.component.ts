import {
  Component,
  InjectionToken,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  inject,
  DestroyRef,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter, switchMap, tap, finalize } from 'rxjs/operators';
import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  IdentityRoleDto,
  IdentityUserDto,
  OrganizationUnitService,
  OrganizationUnitWithDetailsDto,
} from '@volo/abp.ng.identity/proxy';

export interface OrganizationUnitConfig {
  getCurrentUnitsMethodName: string;
  addUnitsMethodName: string;
  addUnitsBodyPropName: string;
  deleteMethodName: string;
  deletionLocalizationKey: string;
}

export const ORGANIZATION_UNIT_CONFIG = new InjectionToken<any>('ORGANIZATION_UNIT_CONFIG');

@Component({
  template: '',
})
export class AbstractOrganizationUnitComponent<T = IdentityUserDto | IdentityRoleDto>
  implements OnChanges
{
  protected readonly config = inject(ORGANIZATION_UNIT_CONFIG, { optional: true }) || {};
  protected readonly organizationUnitService = inject(OrganizationUnitService);
  protected readonly confirmation: ConfirmationService = inject(ConfirmationService);
  public readonly list = inject(ListService);
  protected readonly toasterService = inject(ToasterService);

  readonly #destroyRef = inject(DestroyRef);

  @Input() selectedOrganizationUnit: OrganizationUnitWithDetailsDto;

  currentOrganizationUnits = { items: [] } as PagedResultDto<T>;

  checkedUnits = {} as ABP.Dictionary<boolean>;

  isModalVisible: boolean;

  isModalBusy: boolean;

  @Output() onAdded = new EventEmitter<void>();
  @Output() onDeleted = new EventEmitter<void>();

  constructor() {
    this.list.maxResultCount = 1000;
  }

  ngOnChanges({ selectedOrganizationUnit }: SimpleChanges) {
    if (selectedOrganizationUnit?.firstChange) {
      this.hookToQuery();
    } else if (selectedOrganizationUnit?.currentValue) {
      this.list.get();
    }
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query =>
        this.organizationUnitService[this.config.getCurrentUnitsMethodName](
          this.selectedOrganizationUnit.id,
          query,
        ),
      )
      .subscribe((response: PagedResultDto<T>) => {
        this.currentOrganizationUnits = response;

        this.checkedUnits = {};
        response.items.forEach((item: any) => {
          this.checkedUnits[item.id] = true;
        });
      });
  }

  addUnits() {
    this.isModalBusy = true;
    this.organizationUnitService[this.config.addUnitsMethodName](this.selectedOrganizationUnit.id, {
      [this.config.addUnitsBodyPropName]: Object.keys(this.checkedUnits).filter(
        i => this.checkedUnits[i] === true,
      ),
    })
      .pipe(finalize(() => (this.isModalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
        this.list.get();
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.onAdded.emit();
      });
  }
  delete(unitId: string, unitName: string) {
    this.confirmation
      .warn(this.config.deletionLocalizationKey, 'AbpUi::AreYouSure', {
        messageLocalizationParams: [unitName, this.selectedOrganizationUnit.displayName],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() =>
          this.organizationUnitService[this.config.deleteMethodName](
            this.selectedOrganizationUnit.id,
            unitId,
          ),
        ),
        tap(() => this.list.get()),
        takeUntilDestroyed(this.#destroyRef),
      )
      .subscribe(() => {
        this.toasterService.success('AbpUi::DeletedSuccessfully');
        this.onDeleted.emit();
      });
  }

  openModal() {
    this.list.get();
    this.isModalVisible = true;
  }

  isCheckboxDisabled = (id: string): boolean => {
    return this.currentOrganizationUnits.items.findIndex((item: any) => item.id === id) > -1;
  };
}
