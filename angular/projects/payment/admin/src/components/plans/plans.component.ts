import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { Component, inject, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PlanAdminService, PlanDto } from '@volo/abp.ng.payment/proxy';
import { Observable } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { ePaymentComponents } from '../../enums/components';

@Component({
  selector: 'abp-payment-plans',
  templateUrl: './plans.component.html',
  encapsulation: ViewEncapsulation.None,
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: ePaymentComponents.Plans,
    },
  ],
})
export class PaymentPlansComponent implements OnInit {
  data$: Observable<PagedResultDto<PlanDto>>;

  isModalVisible = false;
  modalBusy = false;

  form: UntypedFormGroup;

  selected: PlanDto;

  protected readonly list = inject(ListService<PagedAndSortedResultRequestDto>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly activatedRoute = inject(ActivatedRoute);
  protected readonly injector = inject(Injector);
  private readonly router = inject(Router);
  private readonly service = inject(PlanAdminService);

  ngOnInit() {
    this.data$ = this.list.hookToQuery(this.service.getList);
  }

  onEdit(id: string) {
    this.service
      .get(id)
      .pipe(take(1))
      .subscribe(selectedPlan => {
        this.selected = selectedPlan;
        this.openModal();
      });
  }

  goToGatewayPlans(id: string) {
    this.router.navigate(['./', id], { relativeTo: this.activatedRoute });
  }

  onDelete(id: string, name: string) {
    this.confirmationService
      .warn('Payment::PlanDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(id).subscribe(() => {
            this.list.get();
            this.toasterService.success('AbpUi::DeletedSuccessfully');
          });
        }
      });
  }

  onAdd() {
    this.selected = {} as PlanDto;
    this.openModal();
  }

  openModal() {
    this.createForm();
    this.isModalVisible = true;
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;

    let stream: Observable<PlanDto>;

    if (this.selected.id) {
      stream = this.service.update(this.selected.id, this.form.value);
    } else {
      stream = this.service.create(this.form.value);
    }

    stream.pipe(finalize(() => (this.modalBusy = false))).subscribe(() => {
      this.list.get();
      this.isModalVisible = false;
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }

  private createForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }
}
