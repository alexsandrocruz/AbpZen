import { Provider } from '@angular/core';
import { PAGE_RENDER_STRATEGY } from '@abp/ng.components/page';
import { LeptonXPageRenderService } from '../services/page-render-strategy.service';

export const PAGE_RENDER_PROVIDER: Provider = {
  provide: PAGE_RENDER_STRATEGY,
  useClass: LeptonXPageRenderService,
};
