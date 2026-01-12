import { ListService, PagedResultDto, CoreModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { CaseService } from '@proxy/case';
import { CaseDto } from '@proxy/case/dtos';

@Component({
  standalone: true,
  selector: 'app-case',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    CoreModule,
    ThemeSharedModule,
  ],
  templateUrl: './case.component.html',
  providers: [ListService],
})
export class CaseComponent implements OnInit {
  readonly list = inject(ListService);
  private caseService = inject(CaseService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  case = { items: [], totalCount: 0 } as PagedResultDto<CaseDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedCase = {} as CaseDto;

  ngOnInit() {
    const streamCreator = (query) => this.caseService.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.case = response;
    });
  }

  createCase() {
    this.selectedCase = {} as CaseDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCase(id: string) {
    this.caseService.get(id).subscribe((case) => {
      this.selectedCase = case;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteCase(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.caseService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      
      caseNumber: [
        this.selectedCase.caseNumber || '', 
        []
      ],
      
      title: [
        this.selectedCase.title || '', 
        []
      ],
      
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedCase.id
      ? this.caseService.update(this.selectedCase.id, this.form.value)
      : this.caseService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
