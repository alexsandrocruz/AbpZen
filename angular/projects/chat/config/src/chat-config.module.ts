import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { ChatIconComponent } from './components/chat-icon.component';
import { provideChatConfig } from './providers/chat-config.provider';

@NgModule({
  imports: [CoreModule, NgbTooltipModule],
  declarations: [ChatIconComponent],
})
export class ChatConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideChatConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<ChatConfigModule> {
    return {
      ngModule: ChatConfigModule,
      providers: [provideChatConfig()],
    };
  }
}
