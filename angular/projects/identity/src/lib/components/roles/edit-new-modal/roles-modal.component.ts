import { Component, inject, Injector, input, OnInit, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { AutofocusDirective, LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import {
  ExtensibleModule,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { IdentityRoleDto, IdentityRoleService } from '@volo/abp.ng.identity/proxy';
import { RoleVisibleChange } from '../role-edit.modal';

@Component({
  standalone: true,
  selector: 'abp-roles-modal',
  templateUrl: './roles-modal.component.html',
  imports: [
    ExtensibleModule,
    ThemeSharedModule,
    LocalizationModule,
    ReactiveFormsModule,
    AutofocusDirective,
  ],
})
export class RolesModalComponent implements OnInit {
  protected readonly service = inject(IdentityRoleService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly injector = inject(Injector);

  form: FormGroup;

  selected = input<IdentityRoleDto>();
  visibleChange = output<RoleVisibleChange>();

  protected buildForm() {
    const data = new FormPropData(this.injector, this.selected());
    this.form = generateFormFromProps(data);
  }

  ngOnInit(): void {
    this.buildForm();
  }

  onVisibleChange(visible: boolean, refresh = false) {
    this.visibleChange.emit({ visible, refresh });
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    let observable: Observable<IdentityRoleDto> = this.service.create(this.form.value);

    if (this.selected()?.id.length > 0) {
      const { id } = this.selected();

      const input = {
        ...this.selected(),
        ...this.form.value,
      };

      observable = this.service.update(id, input);
    }

    observable.subscribe(() => {
      this.onVisibleChange(false, true);
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }
}
