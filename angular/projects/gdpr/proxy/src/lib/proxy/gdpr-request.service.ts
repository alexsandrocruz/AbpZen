import type { DownloadTokenResultDto, GdprRequestDto, GdprRequestInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GdprRequestService {
  apiName = 'Gdpr';
  

  deleteUserData = () =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/gdpr/requests',
    },
    { apiName: this.apiName });
  

  getDownloadToken = (id: string) =>
    this.restService.request<any, DownloadTokenResultDto>({
      method: 'GET',
      url: '/api/gdpr/requests/download-token',
      params: { id },
    },
    { apiName: this.apiName });
  

  getList = (input: GdprRequestInput) =>
    this.restService.request<any, PagedResultDto<GdprRequestDto>>({
      method: 'GET',
      url: '/api/gdpr/requests/list',
      params: { userId: input.userId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  getUserData = (requestId: string, token: string) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/gdpr/requests/data/${requestId}`,
      params: { token },
    },
    { apiName: this.apiName });
  

  isNewRequestAllowed = () =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: '/api/gdpr/requests/is-request-allowed',
    },
    { apiName: this.apiName });
  

  prepareUserData = () =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/gdpr/requests/prepare-data',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
