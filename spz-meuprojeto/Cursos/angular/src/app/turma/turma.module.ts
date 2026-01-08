import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { TurmaRoutingModule } from './turma-routing.module';
import { TurmaComponent } from './turma.component';

@NgModule({
  declarations: [TurmaComponent],
  imports: [
    SharedModule,
    TurmaRoutingModule,
    PageModule,
    ThemeSharedModule,
  ],
})
export class TurmaModule { }
