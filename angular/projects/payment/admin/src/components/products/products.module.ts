import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsComponent } from './products.component';
import { SubscriptionTooltipModule } from '../subscription-tooltip/subscription-tooltip.module';
import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ExtensibleModule } from '@abp/ng.components/extensible';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

@NgModule({
  declarations: [ProductsComponent],
  imports: [
    CommonModule,
    PageModule,
    CoreModule,
    ExtensibleModule,
    ThemeSharedModule,
    SubscriptionTooltipModule,
  ],
})
export class ProductsModule {}
