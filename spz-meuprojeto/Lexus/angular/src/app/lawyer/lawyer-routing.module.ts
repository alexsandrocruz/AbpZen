import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LawyerComponent } from './lawyer.component';
import { authGuard, permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { 
    path: '', 
    component: LawyerComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: 'Lexus.Lawyer',
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LawyerRoutingModule {}
