import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { NewEntity1Service } from '@proxy/sapienza.cursos.new-entity1';
import { NewEntity1Dto } from '@proxy/sapienza.cursos.new-entity1/dtos';

@Component({
  selector: 'app-new-entity1',
  templateUrl: './new-entity1.component.html',
  providers: [ListService],
})
export class NewEntity1Component implements OnInit {
  newEntity1 = { items: [], totalCount: 0 } as PagedResultDto<NewEntity1Dto>;

  isModalOpen = false;
  form: FormGroup;
  selectedNewEntity1 = {} as NewEntity1Dto;

  constructor(
    public readonly list: ListService,
    private newEntity1Service: NewEntity1Service,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    const streamCreator = (query) => this.newEntity1Service.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.newEntity1 = response;
    });
  }

  createNewEntity1() {
    this.selectedNewEntity1 = {} as NewEntity1Dto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editNewEntity1(id: string) {
    this.newEntity1Service.get(id).subscribe((newEntity1) => {
      this.selectedNewEntity1 = newEntity1;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteNewEntity1(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.newEntity1Service.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedNewEntity1.id
      ? this.newEntity1Service.update(this.selectedNewEntity1.id, this.form.value)
      : this.newEntity1Service.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
