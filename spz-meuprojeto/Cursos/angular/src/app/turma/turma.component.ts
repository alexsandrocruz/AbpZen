import { ListService, PagedResultDto, CoreModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { TurmaService } from '@proxy/turma';
import { TurmaDto } from '@proxy/turma/dtos';

@Component({
  standalone: true,
  selector: 'app-turma',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
  ],
  templateUrl: './turma.component.html',
  providers: [ListService],
})
export class TurmaComponent implements OnInit {
  readonly list = inject(ListService);
  private turmaService = inject(TurmaService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  turma = { items: [], totalCount: 0 } as PagedResultDto<TurmaDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedTurma = {} as TurmaDto;

  ngOnInit() {
    const streamCreator = (query) => this.turmaService.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.turma = response;
    });
  }

  createTurma() {
    this.selectedTurma = {} as TurmaDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editTurma(id: string) {
    this.turmaService.get(id).subscribe((turma) => {
      this.selectedTurma = turma;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteTurma(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.turmaService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      nome: [this.selectedTurma.nome || '', []],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedTurma.id
      ? this.turmaService.update(this.selectedTurma.id, this.form.value)
      : this.turmaService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
