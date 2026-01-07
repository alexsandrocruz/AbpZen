import type { CreateUpdateTurmaDto, TurmaDto, TurmaGetListInput } from './dtos/models';
import type { LookupDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TurmaService {
  apiName = 'Default';
  

  create = (input: CreateUpdateTurmaDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TurmaDto>({
      method: 'POST',
      url: '/api/app/turma',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/turma/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TurmaDto>({
      method: 'GET',
      url: `/api/app/turma/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: TurmaGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TurmaDto>>({
      method: 'GET',
      url: '/api/app/turma',
      params: { nome: input.nome, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getTurmaLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>({
      method: 'GET',
      url: '/api/app/turma/turma-lookup',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateTurmaDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TurmaDto>({
      method: 'PUT',
      url: `/api/app/turma/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
