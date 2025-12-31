import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface DownloadTokenResultDto {
  token?: string;
}

export interface GdprRequestDto extends EntityDto<string> {
  creationTime?: string;
  readyTime?: string;
}

export interface GdprRequestInput extends PagedAndSortedResultRequestDto {
  userId: string;
}
