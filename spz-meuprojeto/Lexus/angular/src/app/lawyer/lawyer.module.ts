import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { LawyerRoutingModule } from './lawyer-routing.module';
import { LawyerComponent } from './lawyer.component';

@NgModule({
  declarations: [LawyerComponent],
  imports: [
    SharedModule,
    LawyerRoutingModule,
    PageModule,
    ThemeSharedModule,
  ],
})
export class LawyerModule {}
