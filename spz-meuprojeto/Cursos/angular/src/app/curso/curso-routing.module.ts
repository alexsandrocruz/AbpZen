import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CursoComponent } from './curso.component';
import { authGuard, permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { 
    path: '', 
    component: CursoComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: 'Sapienza.Cursos.Curso',
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CursoRoutingModule {}
