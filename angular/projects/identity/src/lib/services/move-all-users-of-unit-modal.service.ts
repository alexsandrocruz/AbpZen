import { computed, inject, Injectable, signal } from '@angular/core';
import {
  OrganizationUnitService,
  OrganizationUnitWithDetailsDto,
} from '@volo/abp.ng.identity/proxy';
import { finalize, take } from 'rxjs/operators';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Injectable()
export class MoveAllUsersOfUnitModalService {
  protected readonly organizationUnitService = inject(OrganizationUnitService);
  protected readonly confirmationService = inject(ConfirmationService);

  readonly #organizationUnits = signal<OrganizationUnitWithDetailsDto[]>([]);

  visible = signal(false);
  loading = signal(false);
  selected = signal<OrganizationUnitWithDetailsDto | undefined>(undefined);

  name = computed(() => this.selected()?.displayName || '');
  otherOrganizationUnits = computed(() => {
    if (!this.selected() || !this.#organizationUnits()?.length) {
      return [];
    }
    return this.#organizationUnits().filter(x => this.selected().id !== x.id);
  });

  protected showWarning() {
    this.confirmationService
      .warn('AbpIdentity::ThereIsNoUsersCurrentlyInThisRole', 'AbpUi::Warning', {
        hideCancelBtn: true,
        yesText: 'AbpUi::Ok',
      })
      .pipe(take(1))
      .subscribe();
  }

  openMoveModal(v: OrganizationUnitWithDetailsDto | undefined) {
    if (v.userCount === 0) {
      this.showWarning();
      return;
    }

    this.selected.set(v);
    this.visible.set(true);
    this.loadOrganizationUnits();
  }

  closeMoveModal() {
    this.visible.set(false);
    this.selected.set(undefined);
  }

  loadOrganizationUnits() {
    this.loading.set(true);
    this.organizationUnitService
      .getListAll()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(x => {
        this.#organizationUnits.set(x.items);
      });
  }

  moveTheUnit(newParentId: string) {
    this.loading.set(true);
    const id = this.selected().id;
    return this.organizationUnitService
      .moveAllUsers(id, newParentId)
      .pipe(finalize(() => this.loading.set(false)));
  }
}
