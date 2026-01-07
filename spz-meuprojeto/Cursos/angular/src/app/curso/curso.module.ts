import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { CursoRoutingModule } from './curso-routing.module';
import { CursoComponent } from './curso.component';

@NgModule({
  declarations: [CursoComponent],
  imports: [
    SharedModule,
    CursoRoutingModule,
    PageModule,
  ],
})
export class CursoModule {}
