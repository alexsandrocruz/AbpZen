import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewEntity1Component } from './new-entity1.component';
import { authGuard, permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { 
    path: '', 
    component: NewEntity1Component,
    canActivate: [authGuard, permissionGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewEntity1RoutingModule {}
