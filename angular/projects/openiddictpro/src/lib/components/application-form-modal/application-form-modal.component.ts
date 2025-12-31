import { Applications, ApplicationService } from '@volo/abp.ng.openiddictpro/proxy';
import { FormPropData, generateFormFromProps } from '@abp/ng.components/extensible';
import { LocalizationService, SubscriptionService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, EventEmitter, inject, Injector, Input, OnInit, Output } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { Observable, of, Subscription } from 'rxjs';
import {
  defaultApplicationsTypeList,
  defaultApplicationTypes,
} from '../../defaults/default-applications-types';
import { hashSetParser } from '../../utils/hashset-parser';
import { DefaultApplicationsConsentType } from '../../defaults/default-applications-consent-type';
import { ApplicationsService } from '../../services/applications.service';

@Component({
  selector: 'abp-application-form-modal',
  templateUrl: './application-form-modal.component.html',
})
export class ApplicationFormModalComponent implements OnInit {
  protected readonly service = inject(ApplicationService);
  protected readonly applicationsService = inject(ApplicationsService);
  protected readonly localizationService = inject(LocalizationService);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly injector = inject(Injector);
  protected readonly toasterService = inject(ToasterService);

  @Output()
  saved = new EventEmitter<void>();

  @Input()
  selected: Applications.Dtos.ApplicationDto;
  form: UntypedFormGroup;
  formValueChanges$: Subscription;
  types$ = of(defaultApplicationsTypeList);
  consentTypes$ = of(DefaultApplicationsConsentType);
  options = { size: 'lg' };

  protected createForm() {
    const selected = this.selected?.id
      ? {
          ...this.selected,
          redirectUris: hashSetParser(this.selected.redirectUris),
          postLogoutRedirectUris: hashSetParser(this.selected.postLogoutRedirectUris),
          extensionGrantTypes: hashSetParser(this.selected.extensionGrantTypes),
        }
      : { clientType: null };
    const data = new FormPropData(this.injector, selected);
    this.form = generateFormFromProps(data);
  }

  getFormValue() {
    return this.form?.value || {};
  }

  ngOnInit(): void {
    this.createForm();
    this.hideFlowToType();
  }

  visibleChange($event: boolean) {
    this.applicationsService.setModalState($event);
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const id = this.selected?.id;
    const isEditMode = !!id;
    let sub: Observable<Applications.Dtos.ApplicationDto>;
    const parseTextAreaValue = value => {
      return Array.isArray(value) ? value : hashSetParser(value);
    };

    const formValue = {
      ...this.form.value,
      redirectUris: parseTextAreaValue(this.form.value.redirectUris),
      postLogoutRedirectUris: parseTextAreaValue(this.form.value.postLogoutRedirectUris),
      extensionGrantTypes: parseTextAreaValue(this.form.value.extensionGrantTypes),
    };

    if (isEditMode) {
      sub = this.service.update(id, formValue);
    } else {
      sub = this.service.create(formValue);
    }

    sub.subscribe(() => {
      this.applicationsService.setModalState(false);
      this.toasterService.success('AbpUi::SavedSuccessfully');
      this.saved.emit();
    });
  }

  hideFlowToType() {
    const formType = this.form.controls['clientType'];
    this.subscription.removeOne(this.formValueChanges$);
    this.formValueChanges$ = this.subscription.addOne(formType.valueChanges, value => {
      if (value == defaultApplicationTypes.public) {
        this.form.patchValue({
          allowDeviceEndpoint: false,
          allowClientCredentialsFlow: false,
        });
      }
    });
  }

  changeTextToType(displayName: string) {
    const form = this.getFormValue();
    if (form.clientType === defaultApplicationTypes.public) {
      return (
        this.localizationService.instant(displayName) +
        ' (' +
        this.localizationService.instant('AbpOpenIddict::NotAvailableForThisType') +
        ')'
      );
    } else {
      return this.localizationService.instant(displayName);
    }
  }
}
