import type { CreateUpdateCursoDto, CursoDto, CursoGetListInput } from './dtos/models';
import type { LookupDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CursoService {
  apiName = 'Default';
  

  create = (input: CreateUpdateCursoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CursoDto>({
      method: 'POST',
      url: '/api/app/curso',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/curso/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CursoDto>({
      method: 'GET',
      url: `/api/app/curso/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getCursoLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>({
      method: 'GET',
      url: '/api/app/curso/curso-lookup',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: CursoGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CursoDto>>({
      method: 'GET',
      url: '/api/app/curso',
      params: { name: input.name, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateCursoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CursoDto>({
      method: 'PUT',
      url: `/api/app/curso/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
