import { Component, EventEmitter, Injector, Input, OnChanges, Output } from '@angular/core';
import { FormPropData, generateFormFromProps } from '@abp/ng.components/extensible';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ExternalLoginProviderDto, IdentityUserService } from '@volo/abp.ng.identity/proxy';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { finalize } from 'rxjs/operators';
import { Confirmation } from '@abp/ng.theme.shared/lib/models/confirmation';

@Component({
  selector: 'abp-external-login',
  templateUrl: './external-login.component.html',
})
export class ExternalLoginComponent implements OnChanges {
  form: UntypedFormGroup;

  selected: ExternalLoginProviderDto;

  modalBusy = false;

  externalLoginProviders: ExternalLoginProviderDto[];

  selectedExternalLoginProvider: string;

  password: string;

  userNameOrEmailAddress: string;

  @Output()
  dataSave = new EventEmitter<void>();

  protected _visible;

  options: Confirmation.Options = {
    hideCancelBtn: true,
    yesText: 'Ok',
  };

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (this._visible === value) return;

    this._visible = value;
    this.visibleChange.emit(value);
  }

  @Output()
  visibleChange = new EventEmitter<boolean>();

  constructor(
    protected injector: Injector,
    public fb: UntypedFormBuilder,
    protected service: IdentityUserService,
    public confirmationService: ConfirmationService,
  ) {}

  ngOnChanges() {
    if (this._visible) {
      this.buildForm();
    }
  }

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
    this.getExternalLoginProviders();
  }

  getExternalLoginProviders() {
    this.service.getExternalLoginProviders().subscribe(externalUsers => {
      if (externalUsers.length == 0) {
        this.visible = false;
        this.confirmationService.info(
          'AbpIdentity::NoExternalLoginProviderAvailable',
          '',
          this.options,
        );
      } else {
        this.externalLoginProviders = externalUsers;
      }
    });
  }

  save() {
    if (this.modalBusy) return;
    this.service
      .importExternalUser({
        provider: this.selectedExternalLoginProvider,
        userNameOrEmailAddress: this.userNameOrEmailAddress,
        password: this.password,
      })
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.dataSave.emit();
        this.visible = false;
      });
  }
}
