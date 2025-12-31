import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  GetTemplateDefinitionListInput,
  TemplateDefinitionDto,
  TemplateDefinitionService,
} from '@volo/abp.ng.text-template-management/proxy';
import { eTextTemplateManagementComponents } from '../../enums/components';

@Component({
  selector: 'abp-text-templates',
  templateUrl: 'text-templates.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eTextTemplateManagementComponents.TextTemplates,
    },
  ],
})
export class TextTemplatesComponent implements OnInit {
  data: PagedResultDto<TemplateDefinitionDto> = { items: [], totalCount: 0 };

  constructor(
    public readonly list: ListService<ABP.PageQueryParams>,
    protected router: Router,
    protected service: TemplateDefinitionService,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(({ filter: filterText, ...query }) =>
        this.service.getList({ ...query, filterText } as GetTemplateDefinitionListInput),
      )
      .subscribe(res => (this.data = res));
  }

  editContents(record: TemplateDefinitionDto) {
    this.router.navigate([
      `/text-template-management/text-templates/contents${
        record.isInlineLocalized ? '/inline' : ''
      }/${record.name}`,
    ]);
  }
}
