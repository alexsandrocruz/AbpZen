import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CaseRoutingModule } from './case-routing.module';
import { CaseComponent } from './case.component';

@NgModule({
  declarations: [CaseComponent],
  imports: [
    SharedModule,
    CaseRoutingModule,
    PageModule,
    ThemeSharedModule,
  ],
})
export class CaseModule {}
