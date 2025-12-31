import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  EventEmitter,
  inject,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { filter, firstValueFrom, tap } from 'rxjs';
import { CoreModule, TrackByService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { DeleteOrganizationUnitService } from '../../../services';
import { OrganizationUnitDeleteValidator } from './organization-unit-delete.validator';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'abp-delete-organization-unit',
  standalone: true,
  imports: [CoreModule, ThemeSharedModule],
  templateUrl: './delete-organization-unit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteOrganizationUnitComponent implements OnInit {
  protected readonly service = inject(DeleteOrganizationUnitService);
  readonly #fb = inject(FormBuilder);
  readonly #trackByService = inject(TrackByService);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly trackByFn = this.#trackByService.by('id');
  protected readonly toasterService = inject(ToasterService);

  form = this.#fb.group(
    {
      assignType: [false],
      organizationUnitId: [null as string],
    },
    { validators: [OrganizationUnitDeleteValidator()] },
  );

  get assignType() {
    return this.form.controls.assignType;
  }

  get organizationUnitId() {
    return this.form.controls.organizationUnitId;
  }

  @Output() deleted = new EventEmitter<void>();

  readonly #isSubmitted = signal(false);

  ngOnInit(): void {
    this.form.controls.assignType.valueChanges
      .pipe(
        filter(assignType => !assignType),
        tap(() => this.#isSubmitted.set(false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  onVisibleChange($event: boolean) {
    if ($event) {
      return;
    }

    this.service.closeDeleteModal();
  }

  showAssignErrorMessage() {
    return (
      this.assignType.value &&
      !this.organizationUnitId.value &&
      this.form.errors?.organizationUnitId?.required &&
      this.#isSubmitted()
    );
  }

  async save() {
    this.#isSubmitted.set(true);
    if (this.form.invalid) {
      return;
    }

    const newParentId = this.form.value.organizationUnitId;

    const del$ = this.service.deleteTheUnit(newParentId);
    await firstValueFrom(del$);
    this.deleted.emit();
    this.service.closeDeleteModal();
  }
}
