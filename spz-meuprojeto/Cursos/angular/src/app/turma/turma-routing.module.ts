import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TurmaComponent } from './turma.component';
import { authGuard, permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { 
    path: '', 
    component: TurmaComponent,
    canActivate: [authGuard, permissionGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TurmaRoutingModule {}
