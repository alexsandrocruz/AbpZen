import type {
  CreateDirectoryInput,
  DirectoryContentDto,
  DirectoryContentRequestInput,
  DirectoryDescriptorDto,
  DirectoryDescriptorInfoDto,
  MoveDirectoryInput,
  RenameDirectoryInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DirectoryDescriptorService {
  apiName = 'FileManagement';

  create = (input: CreateDirectoryInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DirectoryDescriptorDto>(
      {
        method: 'POST',
        url: '/api/file-management/directory-descriptor',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/file-management/directory-descriptor/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DirectoryDescriptorDto>(
      {
        method: 'GET',
        url: `/api/file-management/directory-descriptor/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getContent = (input: DirectoryContentRequestInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DirectoryContentDto>>(
      {
        method: 'GET',
        url: '/api/file-management/directory-descriptor',
        params: {
          filter: input.filter,
          id: input.id,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (parentId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<DirectoryDescriptorInfoDto>>(
      {
        method: 'GET',
        url: '/api/file-management/directory-descriptor/sub-directories',
        params: { parentId },
      },
      { apiName: this.apiName, ...config },
    );

  move = (input: MoveDirectoryInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DirectoryDescriptorDto>(
      {
        method: 'POST',
        url: '/api/file-management/directory-descriptor/move',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  rename = (id: string, input: RenameDirectoryInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DirectoryDescriptorDto>(
      {
        method: 'POST',
        url: `/api/file-management/directory-descriptor/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
