import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { Component, inject, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { GatewayPlanDto, PlanAdminService, PlanService } from '@volo/abp.ng.payment/proxy';
import { Observable } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { ePaymentComponents } from '../../enums/components';

@Component({
  selector: 'abp-gateway-plans',
  templateUrl: './gateway-plans.component.html',
  encapsulation: ViewEncapsulation.None,
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: ePaymentComponents.GatewayPlans,
    },
  ],
})
export class GatewayPlansComponent implements OnInit {
  data$: Observable<PagedResultDto<GatewayPlanDto>>;

  isModalVisible = false;
  modalBusy = false;

  form: UntypedFormGroup;

  selected: GatewayPlanDto;

  get planId() {
    return this.activatedRoute.snapshot.params.planId;
  }

  protected readonly planService = inject(PlanService);
  protected readonly list = inject(ListService<PagedAndSortedResultRequestDto>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly activatedRoute = inject(ActivatedRoute);
  protected readonly service = inject(PlanAdminService);
  protected readonly injector = inject(Injector);

  ngOnInit() {
    this.data$ = this.list.hookToQuery(input => this.service.getGatewayPlans(this.planId, input));
  }

  onEdit(id: string) {
    this.planService
      .getGatewayPlan(this.planId, id)
      .pipe(take(1))
      .subscribe(selectedGatewayPlan => {
        this.selected = selectedGatewayPlan;
        this.openModal();
      });
  }

  onDelete(gateway: string) {
    this.confirmationService
      .warn('Payment::GatewayPlanDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [gateway],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.deleteGatewayPlan(this.planId, gateway).subscribe(() => {
            this.list.get();
            this.toasterService.success('AbpUi::DeletedSuccessfully');
          });
        }
      });
  }

  onAdd() {
    this.selected = {} as GatewayPlanDto;
    this.openModal();
  }

  openModal() {
    this.createForm();
    this.isModalVisible = true;
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;

    let stream: Observable<void>;

    if (this.selected.gateway) {
      stream = this.service.updateGatewayPlan(this.planId, this.selected.gateway, this.form.value);
    } else {
      stream = this.service.createGatewayPlan(this.planId, this.form.value);
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
