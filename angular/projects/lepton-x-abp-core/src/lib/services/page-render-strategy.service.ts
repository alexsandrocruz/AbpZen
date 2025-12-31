import { Injectable } from '@angular/core';
import { PageParts, PageRenderStrategy } from '@abp/ng.components/page';

@Injectable()
export class LeptonXPageRenderService implements PageRenderStrategy {
  shouldRender(type: string) {
    return type !== PageParts.breadcrumb;
  }
}
