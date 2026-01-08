import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CursoRoutingModule } from './curso-routing.module';
import { CursoComponent } from './curso.component';

@NgModule({
  declarations: [CursoComponent],
  imports: [
    SharedModule,
    CursoRoutingModule,
    PageModule,
    ThemeSharedModule,
  ],
})
export class CursoModule { }
