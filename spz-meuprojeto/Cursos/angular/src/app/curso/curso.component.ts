import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { CursoService } from '@proxy/sapienza.cursos.curso';
import { CursoDto } from '@proxy/sapienza.cursos.curso/dtos';

@Component({
  selector: 'app-curso',
  templateUrl: './curso.component.html',
  providers: [ListService],
})
export class CursoComponent implements OnInit {
  curso = { items: [], totalCount: 0 } as PagedResultDto<CursoDto>;

  isModalOpen = false;
  form: FormGroup;
  selectedCurso = {} as CursoDto;

  constructor(
    public readonly list: ListService,
    private cursoService: CursoService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

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
      
      name: [
        this.selectedCurso.name || '', 
        []
      ],
      
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
