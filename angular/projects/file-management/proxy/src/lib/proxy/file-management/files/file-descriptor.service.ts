import type {
  CreateFileInputWithStream,
  DownloadTokenResultDto,
  FileDescriptorDto,
  FileUploadPreInfoDto,
  FileUploadPreInfoRequest,
  MoveFileInput,
  RenameFileInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FileDescriptorService {
  apiName = 'FileManagement';

  create = (
    directoryId: string,
    inputWithStream: CreateFileInputWithStream,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, FileDescriptorDto>(
      {
        method: 'POST',
        url: '/api/file-management/file-descriptor/upload',
        params: {
          directoryId,
          name: inputWithStream.name,
          overrideExisting: inputWithStream.overrideExisting,
          extraProperties: inputWithStream.extraProperties,
        },
        body: inputWithStream.file,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/file-management/file-descriptor/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  download = (id: string, token: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: `/api/file-management/file-descriptor/download/${id}`,
        params: { token },
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDescriptorDto>(
      {
        method: 'GET',
        url: `/api/file-management/file-descriptor/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getContent = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number[]>(
      {
        method: 'GET',
        url: '/api/file-management/file-descriptor/content',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: `/api/file-management/file-descriptor/download/${id}/token`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (directoryId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<FileDescriptorDto>>(
      {
        method: 'GET',
        url: '/api/file-management/file-descriptor',
        params: { directoryId },
      },
      { apiName: this.apiName, ...config },
    );

  getPreInfo = (input: FileUploadPreInfoRequest[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileUploadPreInfoDto[]>(
      {
        method: 'POST',
        url: '/api/file-management/file-descriptor/pre-upload-info',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  move = (input: MoveFileInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDescriptorDto>(
      {
        method: 'POST',
        url: '/api/file-management/file-descriptor/move',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  rename = (id: string, input: RenameFileInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDescriptorDto>(
      {
        method: 'POST',
        url: `/api/file-management/file-descriptor/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
