import {
  Component,
  DestroyRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  computed,
  inject,
  signal,
} from '@angular/core';
import { TrackByService } from '@abp/ng.core';
import { filter, finalize, firstValueFrom, tap } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { RoleDeleteValidator } from './role-delete.validator';
import { RoleVisibleChange } from '../role-edit.modal';
import { IdentityRoleDto, IdentityRoleService } from '@volo/abp.ng.identity/proxy';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-role-delete',
  templateUrl: './role-delete.component.html',
})
export class RoleDeleteComponent implements OnInit {
  @Input({ required: true })
  visible: boolean;

  @Input({ required: true })
  selected: IdentityRoleDto;

  @Output()
  visibleChange = new EventEmitter<RoleVisibleChange>();

  readonly #isSubmitted = signal(false);
  loading = signal(false);
  #roles = signal<IdentityRoleDto[]>([]);
  roles = computed(
    () =>
      this.#roles().filter(e => !this.selected || e.id !== this.selected?.id) as IdentityRoleDto[],
  );

  get userCount() {
    return this.selected?.userCount || 0;
  }

  get name() {
    return this.selected?.name || '';
  }
  protected readonly trackByService = inject(TrackByService);
  protected readonly trackByFn = this.trackByService.by('id');
  protected readonly service = inject(IdentityRoleService);
  protected readonly fb = inject(FormBuilder);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly toasterService = inject(ToasterService);

  public form = this.fb.group(
    {
      assignType: [false],
      role: [null as string],
    },
    {
      validators: RoleDeleteValidator(),
    },
  );
  get assignType$() {
    return this.form.controls['assignType'].valueChanges;
  }

  get assignType() {
    return this.form.controls.assignType;
  }

  get role() {
    return this.form.controls.role;
  }

  onVisibleChange($event: boolean) {
    this.visibleChange.emit({ visible: $event, refresh: false });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.service
      .getAllList()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(res => this.#roles.set(res.items));

    this.assignType$
      .pipe(
        filter(assignType => !assignType),
        tap(() => this.#isSubmitted.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  showAssignErrorMessage() {
    return (
      this.assignType.value &&
      !this.role.value &&
      this.form.errors?.role?.required &&
      this.#isSubmitted()
    );
  }

  async save() {
    this.#isSubmitted.set(true);
    if (this.form.invalid) {
      return;
    }
    this.loading.set(true);

    const { assignType, role } = this.form.value;
    if (assignType) {
      const moveAllTenants$ = this.service.moveAllUsers(this.selected.id, role);
      await firstValueFrom(moveAllTenants$);
    }
    const delete$ = this.service.delete(this.selected.id);
    await firstValueFrom(delete$);

    this.loading.set(false);

    this.toasterService.success('AbpUi::DeletedSuccessfully');
    this.visibleChange.emit({ visible: false, refresh: true });
  }
}
