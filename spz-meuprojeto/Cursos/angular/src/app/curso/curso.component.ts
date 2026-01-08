import { ListService, PagedResultDto, CoreModule } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ConfirmationService, Confirmation, ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CursoService } from '@proxy/curso';
import { CursoDto } from '@proxy/curso/dtos';

@Component({
  standalone: true,
  selector: 'app-curso',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
  ],
  templateUrl: './curso.component.html',
  providers: [ListService],
})
export class CursoComponent implements OnInit {
  readonly list = inject(ListService);
  private cursoService = inject(CursoService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  curso = { items: [], totalCount: 0 } as PagedResultDto<CursoDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedCurso = {} as CursoDto;

  ngOnInit() {
    const streamCreator = (query) => this.cursoService.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.curso = response;
    });
  }

  createCurso() {
    this.selectedCurso = {} as CursoDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCurso(id: string) {
    this.cursoService.get(id).subscribe((curso) => {
      this.selectedCurso = curso;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteCurso(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.cursoService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedCurso.name || '', []],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedCurso.id
      ? this.cursoService.update(this.selectedCurso.id, this.form.value)
      : this.cursoService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
