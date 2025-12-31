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
import { CommonModule } from '@angular/common';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CoreModule, TrackByService } from '@abp/ng.core';
import { EditionDto, EditionService } from '@volo/abp.ng.saas/proxy';
import { filter, finalize, firstValueFrom, tap } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { EditionDeleteValidator } from './edition-delete.validator';
import { EditionVisibleChange } from './edition-edit.modal';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'abp-edition-delete',
  standalone: true,
  imports: [CommonModule, ThemeSharedModule, CoreModule],
  templateUrl: './edition-delete.component.html',
})
export class EditionDeleteComponent implements OnInit {
  @Input({ required: true })
  visible: boolean;

  @Input({ required: true })
  selected: EditionDto;

  @Output()
  visibleChange = new EventEmitter<EditionVisibleChange>();

  readonly #isSubmitted = signal(false);
  loading = signal(false);
  #editions = signal<EditionDto[]>([]);
  editions = computed(
    () =>
      this.#editions().filter(e => !this.selected || e.id !== this.selected?.id) as EditionDto[],
  );

  get tenantCount() {
    return this.selected?.tenantCount || 0;
  }

  get name() {
    return this.selected?.displayName || '';
  }
  protected readonly trackByService = inject(TrackByService);
  protected readonly trackByFn = this.trackByService.by('id');
  protected readonly editionService = inject(EditionService);
  protected readonly fb = inject(FormBuilder);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly toasterService = inject(ToasterService);

  public form = this.fb.group(
    {
      assignType: [false],
      edition: [null as string],
    },
    {
      validators: EditionDeleteValidator(),
    },
  );
  get assignType$() {
    return this.form.controls['assignType'].valueChanges;
  }

  get assignType() {
    return this.form.controls.assignType;
  }

  get edition() {
    return this.form.controls.edition;
  }

  onVisibleChange($event: boolean) {
    this.visibleChange.emit({ visible: $event, refresh: false });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.editionService
      .getAllList()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(res => this.#editions.set(res));

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
      !this.edition.value &&
      this.form.errors.edition.required &&
      this.#isSubmitted()
    );
  }

  async save() {
    this.#isSubmitted.set(true);
    if (this.form.invalid) {
      return;
    }
    this.loading.set(true);

    const { assignType, edition } = this.form.value;
    if (assignType) {
      const moveAllTenants$ = this.editionService.moveAllTenants(this.selected.id, edition);
      await firstValueFrom(moveAllTenants$);
    }
    const delete$ = this.editionService.delete(this.selected.id);
    await firstValueFrom(delete$);

    this.loading.set(false);

    this.toasterService.success('AbpUi::DeletedSuccessfully');
    this.visibleChange.emit({ visible: false, refresh: true });
  }
}
