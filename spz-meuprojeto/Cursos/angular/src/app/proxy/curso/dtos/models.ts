import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateUpdateCursoDto {
  id?: string;
  name?: string;
}

export interface CursoDto extends FullAuditedEntityDto<string> {
  name?: string;
}

export interface CursoGetListInput extends PagedAndSortedResultRequestDto {
  name?: string;
}
