import { ListService, PagedResultDto, CoreModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { LawyerService } from '@proxy/lawyer';
import { LawyerDto } from '@proxy/lawyer/dtos';

@Component({
  standalone: true,
  selector: 'app-lawyer',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    CoreModule,
    ThemeSharedModule,
  ],
  templateUrl: './lawyer.component.html',
  providers: [ListService],
})
export class LawyerComponent implements OnInit {
  readonly list = inject(ListService);
  private lawyerService = inject(LawyerService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  lawyer = { items: [], totalCount: 0 } as PagedResultDto<LawyerDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedLawyer = {} as LawyerDto;

  ngOnInit() {
    const streamCreator = (query) => this.lawyerService.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.lawyer = response;
    });
  }

  createLawyer() {
    this.selectedLawyer = {} as LawyerDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editLawyer(id: string) {
    this.lawyerService.get(id).subscribe((lawyer) => {
      this.selectedLawyer = lawyer;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteLawyer(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.lawyerService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      
      fullName: [
        this.selectedLawyer.fullName || '', 
        []
      ],
      
      preferredName: [
        this.selectedLawyer.preferredName || '', 
        []
      ],
      
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedLawyer.id
      ? this.lawyerService.update(this.selectedLawyer.id, this.form.value)
      : this.lawyerService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
