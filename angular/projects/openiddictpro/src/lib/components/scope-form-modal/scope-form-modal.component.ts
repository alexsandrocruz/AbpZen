import {
  Component,
  EventEmitter,
  inject,
  Injector,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Scopes, ScopeService } from '@volo/abp.ng.openiddictpro/proxy';
import { FormPropData, generateFormFromProps } from '@abp/ng.components/extensible';
import { UntypedFormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { ScopesService } from '../../services/scopes.service';
import { hashSetParser } from '../../utils/hashset-parser';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-scope-form-modal',
  templateUrl: './scope-form-modal.component.html',
})
export class ScopeFormModalComponent implements OnChanges {
  protected readonly injector = inject(Injector);
  protected readonly scopesService = inject(ScopesService);
  protected readonly service = inject(ScopeService);
  protected readonly toasterService = inject(ToasterService);

  form: UntypedFormGroup;
  @Input()
  selected: Scopes.Dtos.ScopeDto | undefined;
  @Output()
  saved = new EventEmitter<boolean>();
  blueprints = { pattern: 'AbpOpenIddict::TheScopeNameCannotContainSpaces' };
  isModalVisible$ = this.scopesService.isModalVisible$;

  ngOnChanges(changes: SimpleChanges): void {
    this.createForm();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const id = this.selected.id;
    const isEditMode = !!id;
    let sub: Observable<Scopes.Dtos.ScopeDto>;
    const value = { ...this.form.value, resources: hashSetParser(this.form.value.resources) };
    if (isEditMode) {
      sub = this.service.update(id, value);
    } else {
      sub = this.service.create(value);
    }
    sub.subscribe(() => {
      this.scopesService.setModalState(false);
      this.toasterService.success('AbpUi::SavedSuccessfully');
      this.saved.emit();
    });
  }

  visibleChange($event: boolean) {
    this.scopesService.setModalState($event);
  }

  private createForm() {
    const selected = this.selected?.id
      ? { ...this.selected, resources: hashSetParser(this.selected.resources) }
      : {};
    const data = new FormPropData(this.injector, selected);
    this.form = generateFormFromProps(data);
  }
}
