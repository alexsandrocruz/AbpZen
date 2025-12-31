import {
  EXTENSIBLE_FORM_VIEW_PROVIDER,
  FormProp,
  EXTENSIONS_FORM_PROP,
} from '@abp/ng.components/extensible';
import { Component, Inject } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';

@Component({
  selector: 'abp-personal-settings-half-row',
  template: ` <div class="w-50 d-inline">
    <div class="mb-3 ">
      <label [attr.for]="name" class="form-label">{{ displayName | abpLocalization }} </label>
      <input
        type="text"
        [attr.id]="id"
        class="form-control"
        [attr.name]="name"
        [formControlName]="name"
      />
    </div>
  </div>`,
  styles: [],
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
})
export class PersonalSettingsHalfRowComponent {
  public displayName: string;
  public name: string;
  public id: string;
  public formGroup: UntypedFormGroup;

  constructor(@Inject(EXTENSIONS_FORM_PROP) private formProp: FormProp) {
    this.displayName = formProp.displayName;
    this.name = formProp.name;
    this.id = formProp.id;
  }
}
