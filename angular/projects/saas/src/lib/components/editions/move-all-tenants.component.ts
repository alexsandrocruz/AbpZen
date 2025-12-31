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
import { CommonModule } from '@angular/common';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CoreModule, TrackByService } from '@abp/ng.core';
import { EditionDto, EditionService } from '@volo/abp.ng.saas/proxy';
import { finalize, firstValueFrom } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { EditionVisibleChange } from './edition-edit.modal';

@Component({
  selector: 'abp-move-all-tenants',
  standalone: true,
  imports: [CommonModule, ThemeSharedModule, CoreModule],
  templateUrl: './move-all-tenants.component.html',
})
export class MoveAllTenantsComponent implements OnInit {
  @Input({ required: true })
  visible: boolean;

  @Input({ required: true })
  selected: EditionDto;

  @Output()
  visibleChange = new EventEmitter<EditionVisibleChange>();

  loading = signal(false);
  #editions = signal<EditionDto[]>([]);
  editions = computed(
    () =>
      this.#editions().filter(e => !this.selected || e.id !== this.selected?.id) as EditionDto[],
  );

  get name() {
    return this.selected?.displayName || '';
  }
  protected readonly trackByService = inject(TrackByService);
  protected readonly trackByFn = this.trackByService.by('id');
  protected readonly editionService = inject(EditionService);
  protected readonly fb = inject(FormBuilder);
  protected readonly toasterService = inject(ToasterService);

  public form = this.fb.group({
    edition: [''],
  });

  onVisibleChange($event: boolean) {
    this.visibleChange.emit({ visible: $event, refresh: false });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.editionService
      .getAllList()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(res => this.#editions.set(res));
  }

  async save() {
    if (this.form.invalid) {
      return;
    }
    this.loading.set(true);
    const { edition } = this.form.value;
    const moveAllTenants$ = this.editionService.moveAllTenants(this.selected.id, edition);
    await firstValueFrom(moveAllTenants$);

    this.loading.set(false);

    this.toasterService.success('AbpUi::SavedSuccessfully');
    this.visibleChange.emit({ visible: false, refresh: true });
  }
}
