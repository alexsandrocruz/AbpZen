import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { CoreModule, LocalizationModule, TrackByService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { MoveAllUsersOfUnitModalService } from '../../../services';

@Component({
  standalone: true,
  selector: 'abp-move-all-users-of-unit',
  templateUrl: './move-all-users-of-unit.component.html',
  imports: [CoreModule, ThemeSharedModule, ReactiveFormsModule, LocalizationModule],
})
export class MoveAllUsersOfUnitComponent {
  readonly #trackByService = inject(TrackByService);
  readonly #fb = inject(FormBuilder);

  protected readonly toasterService = inject(ToasterService);
  protected readonly trackByFn = this.#trackByService.by('id');
  protected readonly form = this.#fb.group({
    selectedOrganizationUnit: ['', [Validators.required]],
  });

  public readonly service = inject(MoveAllUsersOfUnitModalService);

  @Output() moved = new EventEmitter();

  onVisibleChange($event: boolean) {
    if ($event) {
      return;
    }

    this.service.closeMoveModal();
  }

  async save() {
    if (this.form.invalid) {
      return;
    }

    const { selectedOrganizationUnit } = this.form.value;
    const moved$ = this.service.moveTheUnit(selectedOrganizationUnit);
    await firstValueFrom(moved$);
    this.toasterService.success('AbpUi::SavedSuccessfully');
    this.moved.emit();
    this.service.closeMoveModal();
  }
}
