import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  inject,
  Injector,
  DestroyRef,
  input,
  EventEmitter,
  Output,
} from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  ClaimTypeDto,
  IdentityRoleClaimDto,
  IdentityRoleService,
  IdentityUserClaimDto,
  IdentityUserService,
} from '@volo/abp.ng.identity/proxy';
import { Observable } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { SubscriptionService } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { regexValidator } from '../../validators/regex.validator';

type Claim = IdentityUserClaimDto | IdentityRoleClaimDto;

type ClaimTypes = Claim & { inputType?: string };
@Component({
  selector: 'abp-claim-modal',
  templateUrl: './claim-modal.component.html',
  providers: [SubscriptionService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClaimModalComponent implements OnChanges {
  readonly #destroyRef = inject(DestroyRef);

  protected readonly roleService = inject(IdentityRoleService);
  protected readonly userService = inject(IdentityUserService);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly injector = inject(Injector);
  protected readonly fb = inject(FormBuilder);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  protected _visible;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (this._visible === value) {
      return;
    }

    this._visible = value;
    this.visibleChange.emit(value);
  }

  subject = input<{ id: string; title: string; type: 'roles' | 'users' }>();

  @Output()
  visibleChange = new EventEmitter<boolean>();

  modalBusy = false;

  claimTypes: ClaimTypeDto[];

  inputType = 'String';

  subjectClaims: ClaimTypes[];

  service: IdentityRoleService | IdentityUserService;

  form: FormGroup;

  regex: string;
  regexDescription: string;

  ngOnChanges({ visible, subject }: SimpleChanges) {
    if (subject && subject.currentValue) {
      this.service = subject.currentValue.type === 'roles' ? this.roleService : this.userService;
    }

    if (!visible) {
      return;
    }

    if (visible.currentValue) {
      this.initModal();
      return;
    }
    this.subjectClaims = null;
  }

  initModal() {
    this.buildForm();
    this.getClaimTypeNames();
    this.getSubjectClaims(this.subject());
  }

  get claimType() {
    return this.form.controls.claimType;
  }

  get claimValue() {
    return this.form.controls.claimValue;
  }

  findClaim(claimName: string) {
    return this.claimTypes?.find(claim => claimName === claim.name);
  }

  onKeyDown(event: KeyboardEvent) {
    if (this.inputType === 'Number' && (event.key === 'e' || event.key === 'E')) {
      event.preventDefault();
    }
  }

  resetClaimValidations() {
    this.claimValue.clearValidators();
    this.claimValue.addValidators([
      Validators.required,
      regexValidator(this.regex, this.regexDescription, this.injector),
    ]);
    this.claimValue.updateValueAndValidity();
  }

  claimTypeChange() {
    let subjectInputType: string;
    const claim = this.findClaim(this.claimType.value);
    const inputType = claim.valueTypeAsString;
    this.regex = claim.regex;
    this.regexDescription = claim.regexDescription;

    switch (inputType) {
      case 'Int':
        this.inputType = 'Number';
        subjectInputType = 'Number';
        break;
      case 'Boolean':
        this.inputType = 'Boolean';
        subjectInputType = 'Boolean';
        break;
      case 'String':
        this.inputType = 'String';
        subjectInputType = 'String';
        break;
      case 'DateTime':
        this.inputType = 'datetime-local';
        subjectInputType = 'datetime-local';
        break;
    }

    this.resetClaimValidations();
    return subjectInputType;
  }

  private getClaimTypeNames() {
    this.subscription.addOne(this.service.getAllClaimTypes(), claimTypes => {
      this.claimTypes = claimTypes;
    });
  }

  private getSubjectClaims(subject: { id: string; type: 'roles' | 'users' }) {
    this.subscription.addOne(
      (this.service.getClaims(subject.id) as Observable<Claim[]>).pipe(take(1)),
      claims => {
        this.subjectClaims = claims.map(item => ({
          ...item,
          inputType: this.findClaim(item.claimType)?.valueTypeAsString,
        }));
      },
    );
  }

  addClaim(ngForm: FormGroupDirective) {
    if (this.form.invalid) {
      return;
    }
    const key = this.subject().type === 'roles' ? 'roleId' : 'userId';
    const claim: IdentityRoleClaimDto = {
      [key]: this.subject().id,
      ...this.form.value,
      inputType: this.inputType,
    };

    const { claimType, claimValue } = this.form.value || {};
    const existingClaimIndex = this.subjectClaims.findIndex(
      item => item.claimType === claimType && item.claimValue === claimValue,
    );

    if (existingClaimIndex === -1) {
      this.subjectClaims.push(claim);
    } else {
      //Add error message
      this.confirmationService
        .error('AbpIdentity::ClaimAlreadyExist', 'AbpUi::Error', {
          hideYesBtn: true,
          cancelText: 'AbpUi::Ok',
        })
        .pipe(takeUntilDestroyed(this.#destroyRef))
        .subscribe();
    }

    ngForm.resetForm();
    setTimeout(() => {
      this.form.reset();
    }, 0);
  }

  removeClaim(index) {
    if (!this.subjectClaims[index]) return;

    this.subjectClaims = this.subjectClaims.filter((_, i) => i !== index);
  }

  save() {
    if (this.modalBusy) return;

    this.modalBusy = true;

    this.subscription.addOne(
      this.service.updateClaims(this.subject().id, this.subjectClaims).pipe(
        take(1),
        finalize(() => (this.modalBusy = false)),
      ),
      () => {
        this.visible = false;
        this.toasterService.success('AbpUi::SavedSuccessfully');
      },
    );
  }

  private buildForm() {
    this.form = this.fb.group({
      claimType: ['', [Validators.required]],
      claimValue: [
        '',
        [Validators.required, regexValidator(this.regex, this.regexDescription, this.injector)],
      ],
    });
  }
}
