import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateUpdateTurmaDto {
  id?: string;
  nome?: string;
}

export interface TurmaDto extends FullAuditedEntityDto<string> {
  nome?: string;
}

export interface TurmaGetListInput extends PagedAndSortedResultRequestDto {
  nome?: string;
}
