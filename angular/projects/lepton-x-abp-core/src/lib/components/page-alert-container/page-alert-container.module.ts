import { NgModule } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { SafeHtmlPipe } from '@volo/ngx-lepton-x.core';
import { PageAlertContainerComponent } from './page-alert-container.component';

@NgModule({
  declarations: [PageAlertContainerComponent],
  imports: [CoreModule, ThemeSharedModule, SafeHtmlPipe],
  exports: [PageAlertContainerComponent],
})
export class PageAlertContainerModule {}
