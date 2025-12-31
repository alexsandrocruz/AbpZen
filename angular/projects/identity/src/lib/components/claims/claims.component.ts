import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import {
  Component,
  inject,
  Injector,
  OnInit,
  TemplateRef,
  TrackByFunction,
  ViewChild,
} from '@angular/core';
import { AbstractControl, UntypedFormGroup } from '@angular/forms';
import {
  ClaimTypeDto,
  GetIdentityClaimTypesInput,
  IdentityClaimTypeService,
} from '@volo/abp.ng.identity/proxy';
import { finalize } from 'rxjs/operators';
import { eIdentityComponents } from '../../enums/components';
@Component({
  selector: 'abp-claims',
  templateUrl: './claims.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Claims,
    },
  ],
})
export class ClaimsComponent implements OnInit {
  protected readonly list = inject(ListService<GetIdentityClaimTypesInput>);
  protected readonly service = inject(IdentityClaimTypeService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly injector = inject(Injector);
  protected readonly toasterService = inject(ToasterService);

  data: PagedResultDto<ClaimTypeDto> = { items: [], totalCount: 0 };

  @ViewChild('modalContent') modalContent: TemplateRef<any>;

  form: UntypedFormGroup;

  selected: ClaimTypeDto;

  isModalVisible: boolean;

  modalBusy = false;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  getTypeName(valueType: number) {
    switch (valueType) {
      case 0:
        return 'String';
      case 1:
        return 'Int';
      case 2:
        return 'Boolean';
      case 3:
        return 'DateTime';
      default:
        return valueType;
    }
  }

  openModal() {
    this.buildForm();
    this.isModalVisible = true;
  }

  onAdd() {
    this.selected = {} as ClaimTypeDto;
    this.openModal();
  }

  onEdit(id: string) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.openModal();
    });
  }

  save() {
    if (!this.form.valid || this.modalBusy) return;
    this.modalBusy = true;

    const { id } = this.selected;

    (id
      ? this.service.update(id, {
          ...this.selected,
          ...this.form.value,
        })
      : this.service.create(this.form.value)
    )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.list.get();
        this.isModalVisible = false;
        this.toasterService.success('AbpUi::SavedSuccessfully');
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpIdentity::ClaimTypeDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(id).subscribe(() => {
            this.toasterService.success('AbpUi::DeletedSuccessfully');
            this.list.get();
          });
        }
      });
  }
}
