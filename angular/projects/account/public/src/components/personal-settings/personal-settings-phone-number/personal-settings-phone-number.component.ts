import { ChangeDetectorRef, Component, Inject } from '@angular/core';
import { AbstractControl, UntypedFormGroup, FormGroupDirective } from '@angular/forms';
import {
  EXTENSIBLE_FORM_VIEW_PROVIDER,
  EXTENSIONS_FORM_PROP,
  EXTENSIONS_FORM_PROP_DATA,
  FormProp,
} from '@abp/ng.components/extensible';
import { ConfigStateService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';
import { AccountService, ManageProfileStateService } from '../../../services';
import { ProfileDto } from '@volo/abp.ng.account/public/proxy';

@Component({
  selector: 'abp-personal-settings-phone-number',
  templateUrl: './personal-settings-phone-number.component.html',
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
})
export class PersonalSettingsPhoneNumberComponent {
  public displayName: string;
  public name: string;
  public id: string;
  public isEnablePhoneNumberConfirmation: boolean;
  public initialValue: string;
  public isValueChanged$: Observable<boolean>;
  public isVerified: boolean;
  public modalVisible: boolean;
  public token: string;
  private formGroup: UntypedFormGroup;
  public formControl: AbstractControl;
  modalBusy: boolean;

  constructor(
    @Inject(EXTENSIONS_FORM_PROP) private formProp: FormProp,
    @Inject(EXTENSIONS_FORM_PROP_DATA) private propData: ProfileDto,
    private formGroupDirective: FormGroupDirective,
    private configState: ConfigStateService,
    private accountService: AccountService,
    private toasterService: ToasterService,
    private manageProfileState: ManageProfileStateService,
    private cdr: ChangeDetectorRef,
  ) {
    this.name = formProp.name;
    this.id = formProp.id;
    this.isVerified = propData.phoneNumberConfirmed;

    this.displayName = formProp.displayName;
    this.formGroup = this.formGroupDirective.control;
    this.formControl = this.formGroup.controls[this.name];
    this.isEnablePhoneNumberConfirmation = this.getIsEnablePhoneNumberConfirmation();
    this.initialValue = propData.phoneNumber;
    this.isValueChanged$ = this.formControl.valueChanges.pipe(
      map(value => value !== this.initialValue),
    );
  }

  getIsEnablePhoneNumberConfirmation() {
    return (
      this.configState.getSetting('Abp.Identity.SignIn.EnablePhoneNumberConfirmation') === 'True'
    );
  }

  get userId(): string {
    return this.configState.getDeep('currentUser.id');
  }

  initPhoneNumberConfirmation = () => {
    if (this.formControl.invalid) {
      return;
    }
    const phoneNumber = this.formControl.value;
    const userId = this.userId;
    this.accountService
      .sendPhoneNumberConfirmationToken({
        phoneNumber,
        userId,
      })
      .pipe(tap(() => (this.token = '')))
      .subscribe(this.openModal);
  };

  openModal = () => {
    this.modalVisible = true;
    this.cdr.detectChanges();
  };

  removeModal = () => {
    this.modalVisible = false;
  };

  setPhoneNumberAsConfirmed = () => {
    const profile = { ...this.manageProfileState.getProfile(), phoneNumberConfirmed: true };
    this.manageProfileState.setProfile(profile);
  };

  confirmPhoneNumber() {
    this.accountService
      .confirmPhoneNumber({ token: this.token, userId: this.userId })
      .pipe(tap(this.setPhoneNumberAsConfirmed), tap(this.removeModal))
      .subscribe(() => {
        this.toasterService.success('AbpAccount::Verified', '', { life: 5000 });
      });
  }
}
