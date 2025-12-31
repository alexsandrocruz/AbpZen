import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  computed,
  inject,
  signal,
} from '@angular/core';
import { TrackByService } from '@abp/ng.core';
import { finalize, firstValueFrom } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { RoleVisibleChange } from '../role-edit.modal';
import { IdentityRoleDto, IdentityRoleService } from '@volo/abp.ng.identity/proxy';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-move-all-users',
  templateUrl: './move-all-users.component.html',
})
export class MoveAllUsersComponent implements OnInit {
  @Input({ required: true })
  visible: boolean;

  @Input({ required: true })
  selected: IdentityRoleDto;

  @Output()
  visibleChange = new EventEmitter<RoleVisibleChange>();

  loading = signal(false);
  #roles = signal<IdentityRoleDto[]>([]);
  roles = computed(
    () =>
      this.#roles().filter(e => !this.selected || e.id !== this.selected?.id) as IdentityRoleDto[],
  );

  get name() {
    return this.selected?.name || '';
  }
  protected readonly trackByService = inject(TrackByService);
  protected readonly trackByFn = this.trackByService.by('id');
  protected readonly service = inject(IdentityRoleService);
  protected readonly fb = inject(FormBuilder);
  protected readonly toasterService = inject(ToasterService);

  public form = this.fb.group({
    role: [''],
  });

  onVisibleChange($event: boolean) {
    this.visibleChange.emit({ visible: $event, refresh: false });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.service
      .getAllList()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(res => this.#roles.set(res.items));
  }

  async save() {
    if (this.form.invalid) {
      return;
    }
    this.loading.set(true);
    const { role } = this.form.value;
    const moveAllTenants$ = this.service.moveAllUsers(this.selected.id, role);
    await firstValueFrom(moveAllTenants$);

    this.loading.set(false);

    this.toasterService.success('AbpUi::SavedSuccessfully');

    this.visibleChange.emit({ visible: false, refresh: true });
  }
}
