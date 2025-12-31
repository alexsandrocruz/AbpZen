import { Injectable, computed, signal, inject } from '@angular/core';
import { of } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import {
  OrganizationUnitService,
  OrganizationUnitWithDetailsDto,
} from '@volo/abp.ng.identity/proxy';

@Injectable()
export class DeleteOrganizationUnitService {
  protected readonly serviceProxy = inject(OrganizationUnitService);
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
  count = computed(() => {
    if (!this.selected()) {
      return 0;
    }

    return this.selected().userCount || 0;
  });
  hasMemberOrRole = computed(() => this.count() > 0);
  countText = computed(() => this.count().toString());
  hasOtherUnits = computed(() => !!this.otherOrganizationUnits().length);

  openDeleteModal(v: OrganizationUnitWithDetailsDto | undefined) {
    this.selected.set(v);
    this.visible.set(true);
    this.loadOrganizationUnits();
  }

  closeDeleteModal() {
    this.visible.set(false);
    this.selected.set(undefined);
  }

  loadOrganizationUnits() {
    this.loading.set(true);
    this.serviceProxy
      .getListAll()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(x => {
        this.#organizationUnits.set(x.items);
      });
  }

  deleteTheUnit(newParentId: string | undefined) {
    const id = this.selected().id;
    const move$ = newParentId ? this.serviceProxy.moveAllUsers(id, newParentId) : of(void 0);
    const delete$ = this.serviceProxy.delete(id);

    return move$.pipe(switchMap(() => delete$));
  }
}
